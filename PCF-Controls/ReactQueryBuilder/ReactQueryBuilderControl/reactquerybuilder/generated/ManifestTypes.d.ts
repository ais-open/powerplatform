/*
*This is auto generated from the ControlManifest.Input.xml file
*/

// Define IInputs and IOutputs Type. They should match with ControlManifest.
export interface IInputs {
    initialQuery: ComponentFramework.PropertyTypes.StringProperty;
    isreadonly: ComponentFramework.PropertyTypes.TwoOptionsProperty;
    items: ComponentFramework.PropertyTypes.DataSet;
}
export interface IOutputs {
    initialQuery?: string;
    isreadonly?: boolean;
    queryjson?: string;
    querysql?: string;
    formattedsqlquery?: string;
}
