import { useState, useEffect } from 'react';
import * as React from "react";
import type { Field, RuleGroupType } from 'react-querybuilder';
import { QueryBuilder, formatQuery } from 'react-querybuilder';
import './css/query-builder.css';
import { QueryBuilderDnD } from '@react-querybuilder/dnd';
import * as ReactDnD from 'react-dnd';
import * as ReactDndHtml5Backend from 'react-dnd-html5-backend';

export interface AppProps {
  fields: Field[];
  onQueryChange: (query: RuleGroupType) => void;
  initialQuery?: any;
  isReadOnly: boolean;
  reset: boolean;

}
const initialQueryDefault: RuleGroupType = {
  combinator: 'and',
  rules: [
    // { field: 'firstName', operator: 'beginsWith', value: 'Stev' },
    // { field: 'lastName', operator: 'in', value: 'Vai,Vaughan' },
  ],
};

export const QueryBuilderComponent: React.FC<AppProps> = ({ fields, onQueryChange, initialQuery, isReadOnly,reset }) => {
  /*let initialQueryInput: RuleGroupType = initialQueryDefault;
  if (initialQuery ) {
    initialQueryInput = JSON.parse(initialQuery);
  };*/
  
  let initialQueryInput: RuleGroupType;

try {
  initialQueryInput = initialQuery ? JSON.parse(initialQuery) : initialQueryDefault;
} catch (error) {
  console.error("Invalid JSON provided for initialQuery:", error);
  initialQueryInput = initialQueryDefault;
}
 // let initialQueryInput = initialQuery ? JSON.parse(initialQuery) : initialQueryDefault;

  const [query, setQuery] = useState(initialQueryInput);

  useEffect(() => {
    onQueryChange(query);
  }, [query, onQueryChange]);


    return (
      <QueryBuilderDnD dnd={{ ...ReactDnD, ...ReactDndHtml5Backend }}>
        <QueryBuilder fields={fields} query={query} onQueryChange={setQuery} controlClassnames={{
          queryBuilder: 'queryBuilder-branches',
          dragHandle: 'queryBuilder-dragHandle',
          shiftActions: 'shiftActions'
        }}
        disabled ={isReadOnly}
        showNotToggle
        showCloneButtons
        />
      </QueryBuilderDnD>
    );
  //}
};
