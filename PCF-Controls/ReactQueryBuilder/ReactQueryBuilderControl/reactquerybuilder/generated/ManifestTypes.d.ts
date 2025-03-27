/*
*This is auto generated from the ControlManifest.Input.xml file
*/

// Define IInputs and IOutputs Type. They should match with ControlManifest.
export interface IInputs {
    sampleProperty: ComponentFramework.PropertyTypes.StringProperty;
    initialQuery: ComponentFramework.PropertyTypes.StringProperty;
    fields: ComponentFramework.PropertyTypes.StringProperty;
    isreadonly: ComponentFramework.PropertyTypes.TwoOptionsProperty;
    reset: ComponentFramework.PropertyTypes.TwoOptionsProperty;
    items: ComponentFramework.PropertyTypes.DataSet;
}
export interface IOutputs {
    sampleProperty?: string;
    initialQuery?: string;
    fields?: string;
    isreadonly?: boolean;
    reset?: boolean;
    queryjson?: string;
    querysql?: string;
    formattedsqlquery?: string;
}
