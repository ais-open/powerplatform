import * as React from 'react'
import { ComboBox, IComboBoxOption, IComboBoxStyles } from '@fluentui/react/lib/ComboBox'

export interface IRecordSelectorProps {
    selectedRecordId: string | undefined;
    availableOptions: IComboBoxOption[];
    autoComplete: boolean;
    onChange: (selectedOption?: IComboBoxOption) => void;
}

export const RecordSelector: React.FunctionComponent<IRecordSelectorProps> = (props) => {
  return (
        <ComboBox
            selectedKey={props.selectedRecordId}
            options={props.availableOptions}
            autoComplete={props.autoComplete ? 'on' : 'off'}
            allowFreeform={true}
            onChange={(e: unknown, option?: IComboBoxOption) => {
              props.onChange(option)
            }}
            styles={comboStyles}
        />
  )
}

const colorFocus = '#a9a9a9'

export const comboStyles: Partial<IComboBoxStyles> = {
  root: [
    {
      color: 'black',
      display: 'block',
      fontWeight: '400',
      fontStretch: 'normal',
      borderWidth: '1px',
      borderStyle: 'solid',
      borderColor: 'transparent',
      backgroundColor: 'transparent',
      outline: 'none',
      outlineColor: 'transparent',
      outlineOffset: '0',
      boxSizing: 'border-box',
      height: '33px',
      width: '100%',
      selectors: {
        ':after': {
          outline: 'none',
          border: '1px solid transparent',
          outlineColor: 'transparent',
          boxShadow: 'none'
        },
        ':after:focus': {
          outline: 'none',
          border: `1px solid ${colorFocus}`,
          outlineColor: 'transparent',
          boxShadow: 'none'
        },
        ':focus': {
          outline: 'none',
          border: `1px solid ${colorFocus}`,
          outlineColor: 'transparent',
          boxShadow: 'none'
        }
      }
    }
  ],
  input: [
    {
      outline: 'none',
      border: '1px solid transparent',
      outlineColor: 'transparent',
      outlineOffset: '0',
      boxShadow: 'none'
    }
  ]
}
