---
title: React Query Builder
author: Nick Gill, Anitha Natarajan, Viswanath Vankadari
date: August 9, 2024
---

# Introduction
The solution integrates a custom PCF control using React Query Builder and SQL Formatter within Power App. This control offers an intuitive interface for building SQL queries in real-time, faster, and with fewer errors. The ability to format and share queries makes the process more efficient, reducing bottlenecks and improving collaboration.

# React Query Builder
The React Query Builder is a UI component that allows users to construct complex queries visually. It abstracts the complexity of writing query conditions manually, offering a more interactive and user-friendly interface. The builder is commonly used for:

Filtering data: Users can apply filters to datasets based on various attributes.
Advanced search: Enabling custom search conditions, like filtering by ranges, multiple conditions, and various data types.
Custom queries: Allowing users to define rules based on fields, operators, and values.

# SQL Formatter
sql-formatter is an npm package that provides an easy-to-use utility for formatting SQL queries. It's designed to improve the readability and consistency of SQL code by automatically formatting queries according to customizable rules. 

repo link

| Canvas apps | Custom pages | Model-driven apps | Portals |
| ----------- | ------------ | ----------------- | ------- |
| ✅           | ✅            | ⬜                 | ⬜       |

## Properties

### Input Properties

| Property name | Display name | Type  | Description |
| -------- | ----------- | -------------- | ---------- |
| `Items` (required) | Input Table | Table | Table containing column internal name, column display name and data type |
| `initialQuery` | Default query | Table | Default query to display in a PowerFx table format |
| `isReadOnly` | Display mode | Boolean | Property to define the display mode of the control|
| `Reset` | Reset | Boolean | Property to reset the state of the control |


### Output Properties

| Property name | Display name | Type | Description |
| -------- | ----------- | -------------- | ---------- |
| `querysql` | Query clause | String | The generated where clause of the query. |
| `formattedquerysql` | Formatted query clause | String | The generated formatted where clause of the query |

### Notes

For a more complete introduction, see the [main package README](packages/react-querybuilder/README.md), dive into the [full documentation](https://react-querybuilder.js.org/docs/intro), or browse the [example projects](./examples/)