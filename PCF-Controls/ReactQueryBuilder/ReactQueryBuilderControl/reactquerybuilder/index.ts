import { IInputs, IOutputs } from "./generated/ManifestTypes";
import * as React from "react";
import { createRoot, Root } from 'react-dom/client';
import { QueryBuilderComponent } from "./QueryBuilderComponent";
import './css/query-builder.css';
import { AppProps } from "./QueryBuilderComponent";
import { defaultValueProcessorByRule, Field, formatQuery, RuleGroupType, ValueProcessorByRule } from 'react-querybuilder';
import { getFieldsFromItemsDataset } from "./DatasetMapping";
import { format } from 'sql-formatter';

export class reactquerybuilder implements ComponentFramework.StandardControl<IInputs, IOutputs> {
    private notifyOutputChanged: () => void;
    private container: HTMLDivElement;
    private appProps: AppProps;
    private items: Field[] = [];
    private stringFields: Field[] = [];
    private query: RuleGroupType | null = null;
    private initialQuery: RuleGroupType | null = null;
    private previousInitialQueryRaw: string | null = null;
    private root: Root;

    constructor() {

    }

    /**
     * Used to initialize the control instance. Controls can kick off remote server calls and other initialization actions here.
     * Data-set values are not initialized here, use updateView.
     * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to property names defined in the manifest, as well as utility functions.
     * @param notifyOutputChanged A callback method to alert the framework that the control has new outputs ready to be retrieved asynchronously.
     * @param state A piece of data that persists in one session for a single user. Can be set at any point in a controls life cycle by calling 'setControlState' in the Mode interface.
     * @param container If a control is marked control-type='standard', it will receive an empty div element within which it can render its content.
     */
    public init(context: ComponentFramework.Context<IInputs>, notifyOutputChanged: () => void, state: ComponentFramework.Dictionary, container: HTMLDivElement): void {
        this.notifyOutputChanged = notifyOutputChanged;
        this.container = container;
        this.root = createRoot(this.container);
    }

    /**
     * Called when any value in the property bag has changed. This includes field values, data-sets, global values such as container height and width, offline status, control metadata values such as label, visible, etc.
     * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to names defined in the manifest, as well as utility functions
     */
    public updateView(context: ComponentFramework.Context<IInputs>): void {
        // Add code to update control view
        const dataset = context.parameters.items;
        this.items = getFieldsFromItemsDataset(dataset);
        this.stringFields = this.items.filter(a => a.inputType === "text");
        
        // Check if initialQuery has changed
        const initialQueryRaw = context.parameters.initialQuery?.raw || null;
        const initialQueryChanged = initialQueryRaw !== this.previousInitialQueryRaw;
        this.previousInitialQueryRaw = initialQueryRaw;
        
        // Parse initialQuery safely
        try {
            this.initialQuery = initialQueryRaw ? JSON.parse(initialQueryRaw) : null;
        } catch (error) {
            console.error("Failed to parse initialQuery JSON:", error);
            this.initialQuery = null;
        }

        this.appProps = {
            fields: this.items,
            onQueryChange: this.handleQueryChange.bind(this),
            initialQuery: this.initialQuery,
            initialQueryChanged: initialQueryChanged,
            isReadOnly: context.parameters.isreadonly.raw === true
        }

        this.root.render(
            React.createElement(QueryBuilderComponent, this.appProps));
        this.notifyOutputChanged();
    }
    
    private handleQueryChange(queryOutput: RuleGroupType): void {
        this.query = queryOutput;
        this.notifyOutputChanged();
    }

    /**
     * Formats a comma-separated string into a SQL IN clause format with proper quotes
     * @param input A comma-separated string of values
     * @returns A properly formatted string for SQL IN clauses with parentheses and quotes
     */
    private getQuoteString(input: string): string {
        // Split the input string by commas
        const items = input.split(',');

        // Map each item to wrap it in single quotes and return the formatted string
        const quotedItems = items.map((item: string) => `'${item.trim()}'`);

        // Join the quoted items back into a single string separated by commas
        return "(" + quotedItems.join(',') + ")";
    }

    /**
     * Creates a custom value processor for SQL query formatting
     * @returns A ValueProcessorByRule function that properly formats string values in SQL queries
     */
    private getCustomValueProcessor(): ValueProcessorByRule {
        return (rule, options) => {
            // Only process specific operators that need string handling
            const stringOperators = ["in", "notIn", "=", "!="];
            
            if (stringOperators.includes(rule.operator) && 
                this.isStringField(rule.field) && 
                this.hasValue(rule.value)) {
                
                const value = rule.value.toString();
                
                // Handle IN/NOT IN operators differently from equals operators
                if (rule.operator === "in" || rule.operator === "notIn") {
                    return this.getQuoteString(value);
                } else {
                    return `'${value}'`;
                }
            }
            
            // Fall back to default processor for all other cases
            return defaultValueProcessorByRule(rule, options);
        };
    }
    
    /**
     * Checks if a field is a string field
     * @param fieldName Name of the field to check
     * @returns True if the field is a string field, false otherwise
     */
    private isStringField(fieldName: string): boolean {
        return this.stringFields.some(field => field.name === fieldName);
    }
    
    /**
     * Checks if a value exists and is not empty
     * @param value The value to check
     * @returns True if the value exists and is not an empty string
     */
    private hasValue(value: any): boolean {
        return value !== undefined && value.toString().trim() !== '';
    }

    /**
     * It is called by the framework prior to a control receiving new data.
     * @returns an object based on nomenclature defined in manifest, expecting object[s] for property marked as "bound" or "output"
     */
    public getOutputs(): IOutputs {
        // If there's no query, return empty strings for all outputs
        if (!this.query) {
            return {
                queryjson: '',
                querysql: '',
                formattedsqlquery: ''
            };
        }

        // Get the JSON formatted query
        const queryJson = formatQuery(this.query, { format: 'json', parseNumbers: false });
        
        // Get the SQL query with custom value processor
        const sqlQuery = formatQuery(this.query, { 
            format: 'sql', 
            parseNumbers: true, 
            valueProcessor: this.getCustomValueProcessor() 
        });
        
        // Format the SQL query for readability
        const formattedSqlQuery = format(sqlQuery, {
            language: 'sql',
            tabWidth: 2, 
            keywordCase: 'preserve', 
            dataTypeCase: 'preserve',
            functionCase: 'preserve', 
            identifierCase: 'preserve', 
            indentStyle: 'standard',
            logicalOperatorNewline: 'before'
        });
        
        return {
            queryjson: queryJson,
            querysql: sqlQuery,
            formattedsqlquery: formattedSqlQuery
        };
    }

    /**
     * Called when the control is to be removed from the DOM tree. Controls should use this call for cleanup.
     * i.e. cancelling any pending remote calls, removing listeners, etc.
     */
    public destroy(): void {
        if (this.root) {
            this.root.unmount();
        }
    }
}
