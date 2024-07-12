/* eslint-disable @typescript-eslint/no-empty-function */
/* eslint-disable no-mixed-spaces-and-tabs */
/* eslint-disable no-tabs */
import { IInputs, IOutputs } from './generated/ManifestTypes'
import * as React from 'react'
import * as ReactDom from 'react-dom'
import { RecordSelector } from './RecordSelector'
import { IComboBoxOption } from '@fluentui/react/lib/ComboBox'

export class LookupToCombobox implements ComponentFramework.StandardControl<IInputs, IOutputs> {
    private _container: HTMLDivElement;
    private _notifyOutputChanged: () => void;
    private _entityName: string;
    private _viewId: string;
    private _displayNameField: string;
    private _availableOptions: IComboBoxOption[];
    private _currentValue?: ComponentFramework.LookupValue[];
    private _autoComplete: boolean;
    private _logString: string;
    constructor () {      
    }
    
    public init (
      context: ComponentFramework.Context<IInputs>,
      notifyOutputChanged: () => void,
      state: ComponentFramework.Dictionary,
      container: HTMLDivElement
    ): void {
      this._container = container
      this._notifyOutputChanged = notifyOutputChanged
      this._logString = `[LookupToComboBox(${(<any>context).navigation._customControlProperties.controlId})]`
      console.debug(`${this._logString} Control initializing, getting target entity type from lookup`)

      this._entityName = context.parameters.lookup.getTargetEntityType()
      this._viewId = context.parameters.lookupViewGuid.raw ?? 'No View Specified'
      this._displayNameField = context.parameters.displayNameField.raw ?? 'No Display Name Specified'
      this._autoComplete = context.parameters.autoComplete.raw == '1'

      console.debug(
            `${this._logString} Target entity type [${this._entityName}] and view id [${this._viewId}] retrieved from context, getting entity metadata`
      )

      context.utils.getEntityMetadata(this._entityName).then((metadata) => {
        const entityIdFieldName = metadata.PrimaryIdAttribute
        const entityNameFieldName = this._displayNameField // context.parameters.DisplayNameField.raw ?? metadata.PrimaryNameAttribute;

        console.debug(`${this._logString} Using view id [${this._viewId}] to retrieve fetchXML for view`)

        context.webAPI
          .retrieveRecord('savedquery', this._viewId, '?$select=name,fetchxml')
          .then((queryFetchXml) => {
            // const query = `?$select=${entityIdFieldName},${entityNameFieldName}`;
            const query = '?fetchXml=' + queryFetchXml.fetchxml
            console.info(`${this._logString} Retrieving multiple records using fetchXml [${query}]`)

            context.webAPI.retrieveMultipleRecords(this._entityName, query).then((result) => {
              this._availableOptions = result.entities.map((r) => {
                return {
                  key: r[entityIdFieldName],
                  text: r[entityNameFieldName] ?? 'Display Name is not available'
                }
              })

              this.renderControl(context)
            })
          })
      })
    }

    public updateView (context: ComponentFramework.Context<IInputs>): void {
      this.renderControl(context)
    }

    private renderControl (context: ComponentFramework.Context<IInputs>) {
      const recordId =
            context.parameters.lookup.raw != null && context.parameters.lookup.raw.length > 0
              ? context.parameters.lookup.raw[0].id
              : undefined

      const recordSelector = React.createElement(RecordSelector, {
        selectedRecordId: recordId,
        availableOptions: this._availableOptions,
        autoComplete: this._autoComplete,
        onChange: (selectedOption?: IComboBoxOption) => {
          if (typeof selectedOption === 'undefined') {
            this._currentValue = undefined
          } else {
            this._currentValue = [
              {
                id: <string>selectedOption.key,
                name: selectedOption.text,
                entityType: this._entityName
              }
            ]
          }
          this._notifyOutputChanged()
        }
      })

      ReactDom.render(recordSelector, this._container)
    }

    public getOutputs (): IOutputs {
      return {
        lookup: this._currentValue
      }
    }

    public destroy (): void {
      ReactDom.unmountComponentAtNode(this._container)
    }
}
