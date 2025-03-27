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
  initialQueryChanged?: boolean;
  isReadOnly: boolean;
  reset: boolean;
}

const initialQueryDefault: RuleGroupType = {
  combinator: 'and',
  rules: [],
};

export const QueryBuilderComponent: React.FC<AppProps> = ({ 
  fields, 
  onQueryChange, 
  initialQuery, 
  initialQueryChanged,
  isReadOnly, 
  reset 
}) => {
  // Parse initial query safely
  const parseInitialQuery = React.useCallback(() => {
    if (!initialQuery) {
      return initialQueryDefault;
    }

    if (reset) {
      return initialQueryDefault;
    }

    // If initialQuery is already an object, use it directly
    if (typeof initialQuery === 'object' && initialQuery !== null) {
      return initialQuery;
    }

    try {
      // Otherwise parse it as a JSON string
      return JSON.parse(initialQuery);
    } catch (error) {
      console.error("Failed to parse initialQuery:", error);
      return initialQueryDefault;
    }
  }, [initialQuery, reset]);

  // Set up query state with the parsed initial query
  const [query, setQuery] = useState(parseInitialQuery());
  
  // Update query when initialQuery changes (detected by initialQueryChanged flag)
  useEffect(() => {
    if (initialQueryChanged || reset) {
      console.log("Query reset due to initialQuery change or reset flag");
      setQuery(parseInitialQuery());
    }
  }, [initialQueryChanged, reset, parseInitialQuery]);

  // Notify parent of query changes
  useEffect(() => {
    onQueryChange(query);
  }, [query, onQueryChange]);

  return (
    <QueryBuilderDnD dnd={{ ...ReactDnD, ...ReactDndHtml5Backend }}>
      <QueryBuilder 
        fields={fields} 
        query={query} 
        onQueryChange={setQuery} 
        controlClassnames={{
          queryBuilder: 'queryBuilder-branches',
          dragHandle: 'queryBuilder-dragHandle',
          shiftActions: 'shiftActions'
        }}
        disabled={isReadOnly}
        showNotToggle
        showCloneButtons
      />
    </QueryBuilderDnD>
  );
};
