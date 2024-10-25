import { Field } from "react-querybuilder";

export function getFieldsFromItemsDataset(dataset: ComponentFramework.PropertyTypes.DataSet): Field[] {
  const fieldItems: Field[] = [];
  return dataset.sortedRecordIds.map((recordId) => {
    const record = dataset.records[recordId];
    const inputType = record.getValue("inputType");

    const field = {
      name: record.getValue("name"),
      label: record.getValue("label"),
      inputType: inputType,
    };

    if (inputType === "checkbox") {
      (field as Field).valueEditorType = 'checkbox';
      (field as Field).operators = [
        { name: '=', value: '=', label: '=' },
        { name: '!=', value: '!=', label: '!=' }
      ];
    }

   else if (inputType === "text") {
      (field as Field).operators = [
        { name: '=', value: '=', label: '=' },
        { name: '!=', value: '!=', label: '!=' },
        { name: 'contains', value: 'contains', label: 'contains' },
        { name: 'beginsWith', value: 'beginsWith', label: 'begins with' },
        { name: 'endsWith', value: 'endsWith', label: 'ends with' },
        { name: 'doesNotContain', value: 'doesNotContain', label: 'does not contain' },
        { name: 'doesNotBeginWith', value: 'doesNotBeginWith', label: 'does not begin with' },
        { name: 'doesNotEndWith', value: 'doesNotEndWith', label: 'does not end with' },
        { name: 'null', value: 'null', label: 'is null' },
        { name: 'notNull', value: 'notNull', label: 'is not null' },
        { name: 'in', value: 'in', label: 'in' },
        { name: 'notIn', value: 'notIn', label: 'not in' },
      ]
    }

   else if (inputType === "number" || inputType === "date") {
      (field as Field).operators = [
        { name: '=', value: '=', label: '=' },
        { name: '!=', value: '!=', label: '!=' },
        { name: '<', value: '<', label: '<' },
        { name: '>', value: '>', label: '>' },
        { name: '<=', value: '<=', label: '<=' },
        { name: '>=', value: '>=', label: '>=' },
        { name: 'null', value: 'null', label: 'is null' },
        { name: 'notNull', value: 'notNull', label: 'is not null' },
        { name: 'in', value: 'in', label: 'in' },
        { name: 'notIn', value: 'notIn', label: 'not in' },
        { name: 'between', value: 'between', label: 'between' },
        { name: 'notBetween', value: 'notBetween', label: 'not between' },
      ]
    }
    return field as Field;
  });
}