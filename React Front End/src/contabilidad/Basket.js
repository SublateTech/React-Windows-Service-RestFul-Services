import React from 'react';

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

import _ from "lodash";

import { makeData, Logo, Tips } from "./Utils";

const columns = [
    {
      Header: "Name",
      columns: [
        {
          Header: "First Name",
          accessor: "firstName"
        },
        {
          Header: "Last Name",
          id: "lastName",
          accessor: d => d.lastName
        }
      ]
    },
    {
      Header: "Info",
      columns: [
        {
          Header: "Age",
          accessor: "age"
        },
        {
          Header: "Status",
          accessor: "status"
        }
      ]
    },
    {
      Header: "Stats",
      columns: [
        {
          Header: "Visits",
          accessor: "visits"
        }
      ]
    }
  ];

export const SubComponent10K = class SubComponent10k extends React.Component {
    constructor() {
      super();
      this.state = {
        data: makeData(10000)
      };
    }
    render() {
      const { data } = this.state;
      return (
        <div>
          <br />
          <strong>Note: Having the console open will slow performance</strong>
          <br />
          <br />
  
          <ReactTable
            data={data}
            columns={[
              {
                Header: "Name",
                columns: [
                  {
                    Header: "First Name",
                    accessor: "firstName"
                  },
                  {
                    Header: "Last Name",
                    id: "lastName",
                    accessor: d => d.lastName
                  }
                ]
              },
              {
                Header: "Info",
                columns: [
                  {
                    Header: "Age",
                    accessor: "age",
                    aggregate: vals => _.round(_.mean(vals)),
                    Aggregated: row => {
                      return (
                        <span>
                          {row.value} (avg)
                        </span>
                      );
                    }
                  },
                  {
                    Header: "Visits",
                    accessor: "visits",
                    aggregate: vals => _.sum(vals)
                  }
                ]
              }
            ]}
            pivotBy={["firstName", "lastName"]}
            defaultPageSize={10}
            className="-striped -highlight"
            SubComponent={row => {
              return (
                <div style={{ padding: "20px" }}>
                  <em>Sub Component!</em>
                </div>
              );
            }}
          />
          <br />
          <Tips />
          <Logo />
        </div>
      );
    }
  }

  export const SubComponents = class SubComponents extends React.Component {
    constructor() {
      super();
      this.state = {
        data: makeData()
      };
    }
    render() {
      const { data } = this.state;
      return (
        <div>
          <ReactTable
            data={data}
            columns={columns}
            defaultPageSize={10}
            className="-striped -highlight"
            SubComponent={row => {
              return (
                <div style={{ padding: "20px" }}>
                  <em>
                    You can put any component you want here, even another React
                    Table!
                  </em>
                  <br />
                  <br />
                  <ReactTable
                    data={data}
                    columns={columns}
                    defaultPageSize={3}
                    showPagination={false}
                    SubComponent={row => {
                      return (
                        <div style={{ padding: "20px" }}>
                          Another Sub Component!
                        </div>
                      );
                    }}
                  />
                </div>
              );
            }}
          />
          <br />
          <Tips />
          <Logo />
        </div>
      );
    }
  }

  

class Basket extends React.Component {
    constructor() {
        super();
        this.state = {
          data: makeData()
        };
      }
      render() {
        const { data } = this.state;
        return (
          <div>
            <ReactTable
              data={data}
              columns={[{
          Header: 'Name',
          columns: [{
            Header: 'First Name',
            accessor: 'firstName'
          }, {
            Header: 'Last Name',
            id: 'lastName',
            accessor: d => d.lastName
          }]
        }, {
          Header: 'Info',
          columns: [{
            Header: 'Profile Progress',
            accessor: 'progress',
            Cell: row => (
              <div
                style={{
                  width: '100%',
                  height: '100%',
                  backgroundColor: '#dadada',
                  borderRadius: '2px'
                }}
              >
                <div
                  style={{
                    width: `${row.value}%`,
                    height: '100%',
                    backgroundColor: row.value > 66 ? '#85cc00'
                      : row.value > 33 ? '#ffbf00'
                      : '#ff2e00',
                    borderRadius: '2px',
                    transition: 'all .2s ease-out'
                  }}
                />
              </div>
            )
          }, {
            Header: 'Status',
            accessor: 'status',
            Cell: row => (
              <span>
                <span style={{
                  color: row.value === 'relationship' ? '#ff2e00'
                    : row.value === 'complicated' ? '#ffbf00'
                    : '#57d500',
                  transition: 'all .3s ease'
                }}>
                  &#x25cf;
                </span> {
                  row.value === 'relationship' ? 'In a relationship'
                  : row.value === 'complicated' ? `It's complicated`
                  : 'Single'
                }
              </span>
            )
          }]
        }]}
              defaultPageSize={10}
              className="-striped -highlight"
            />
            <br />
            <Tips />
            <Logo />
          </div>
        );
      }
    
  }
  


export default Basket;