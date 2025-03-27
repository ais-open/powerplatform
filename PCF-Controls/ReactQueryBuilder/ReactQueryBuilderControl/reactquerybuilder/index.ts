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
    private items: Field[];
    private stringFields: Field[];
    private query: RuleGroupType;
    private initialQuery: RuleGroupType;
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
        this.stringFields = this.items.filter(a => a.inputType == "text");
        const initialQueryRaw = context.parameters.initialQuery;
        this.initialQuery = initialQueryRaw && initialQueryRaw.raw ? JSON.parse(initialQueryRaw.raw) : null;

        this.appProps = {
            fields: this.items,
            onQueryChange: this.handleQueryChange.bind(this),
            initialQuery: this.initialQuery,
            isReadOnly: context.parameters.isreadonly.raw == true,
            reset: context.parameters.reset.raw == true
        }

        this.root.render(
            React.createElement(QueryBuilderComponent, this.appProps));
        this.notifyOutputChanged();
    }
    private handleQueryChange(queryOutput: RuleGroupType): void {
        this.query = queryOutput;
        this.notifyOutputChanged();
    }

    getQuoteString = (input: string): string => {
        // Split the input string by commas
        const items = input.split(',');

        // Map each item to wrap it in single quotes and return the formatted string
        const quotedItems = items.map((item: string) => `'${item.trim()}'`);

        // Join the quoted items back into a single string separated by commas
        return "(" + quotedItems.join(',') + ")";
    };

    private getCustomValueProcessor(): ValueProcessorByRule {
        return (rule, options) => {
            if (rule.operator === "in" ||
                rule.operator === "notIn" ||
                rule.operator === "=" ||
                rule.operator === "!="
            ) {
                const columnIndex = this.stringFields.findIndex(a => a.name === rule.field);

                if (columnIndex >= 0) {
                    const str = rule.value;
                    if (rule.value != undefined && rule.value.toString().trim() != '') {
                        if (rule.operator === "in" || rule.operator === "notIn") {
                            const quoteString = this.getQuoteString(str)
                            return quoteString;
                        }
                        else return "'" + str + "'";
                    }
                }
            }
            return defaultValueProcessorByRule(rule, options);
        };
    }

    /**
     * It is called by the framework prior to a control receiving new data.
     * @returns an object based on nomenclature defined in manifest, expecting object[s] for property marked as "bound" or "output"
     */
    public getOutputs(): IOutputs {
        const customValueProcessor = this.getCustomValueProcessor();
        
        const sqlQuery = this.query ? formatQuery(this.query, { 
            format: 'sql', 
            parseNumbers: true, 
            valueProcessor: customValueProcessor 
        }) : '';
        
        return {
            queryjson: this.query ? formatQuery(this.query, { format: 'json', parseNumbers: false }) : '',
            querysql: sqlQuery,
            formattedsqlquery: format(sqlQuery, {
                language: 'sql',
                tabWidth: 2, keywordCase: 'preserve', dataTypeCase: 'preserve',
                functionCase: 'preserve', identifierCase: 'preserve', indentStyle: 'standard',
                logicalOperatorNewline: 'before'
            })
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
