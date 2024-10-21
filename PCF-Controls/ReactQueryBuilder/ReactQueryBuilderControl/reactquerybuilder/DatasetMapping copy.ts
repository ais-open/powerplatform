import { Field } from "react-querybuilder";

export function getFieldsFromItemsDataset(dataset: ComponentFramework.PropertyTypes.DataSet): Field[] {
  const fieldItems: Field[] = [];
  
  return dataset.sortedRecordIds.map((recordId) => {
    const record = dataset.records[recordId];
    return {
      name: record.getValue("name"),
      label: record.getValue("label"),
      inputType: record.getValue("inputType"),
    } as Field;
  });
}