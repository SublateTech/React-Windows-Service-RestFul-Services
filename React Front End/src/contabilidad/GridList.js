import React from 'react';
import { GridList as MuiGridList } from 'material-ui/GridList';
import {TextField, NumberField } from 'admin-on-rest';
import { Link } from 'react-router-dom';
import {
    Table,
    TableBody,
    TableHeader,
    TableHeaderColumn,
    TableRow,
    TableRowColumn,
  } from 'material-ui/Table';
  
// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

import { makeData, Tips } from "./Utils"; //Logo, 

import _ from "lodash";
//import ExcelButton from './ExcelButton';
//import PDFButton from './PDFButton';
import './GridList.css';
import { stringify } from 'query-string';


const styles = {
    root: {
        margin: '-2px',
    },
    gridList: {
        width: '100%',
        margin: 0,
    },
};

const colored = WrappedComponent => props => props.record[props.source] > 500 ?
    <span style={{ color: 'red' }}><WrappedComponent {...props} /></span> :
    <WrappedComponent {...props} />;

const ColoredNumberField = colored(NumberField);
ColoredNumberField.defaultProps = NumberField.defaultProps;


export const GridList1 = ({ ids, isLoading, data, currentSort, basePath, rowStyle }) => (
    <div style={styles.root}>
        <MuiGridList cellHeight={180} cols={4} style={styles.gridList}>
            {ids.map((id) => (
                <div>
                <TextField record = {data[id]} source="Descripcion"/>
                <ColoredNumberField record = {data[id]}  source="Todos" label="Todos los Hoteles" options={{ style: 'currency', currency: 'USD' }} />    
                <NumberField record = {data[id]} source="H1" label="SALKANTAY" style={{ color: 'purple' }} />
                <NumberField record = {data[id]} source="H2" label="LARES" style={{ color: 'purple' }} />        
                <NumberField record = {data[id]} source="H3" label="EL MERCADO" style={{ color: 'purple' }} />    
                <NumberField record = {data[id]} source="H4" label="VIÑAK" style={{ color: 'purple' }} />    
                <NumberField record = {data[id]} source="H5" label="CORPORATIVO" style={{ color: 'purple' }} />    
                <NumberField record = {data[id]} source="H6" label="HUACAHUASI" style={{ color: 'purple' }} />    
                <NumberField record = {data[id]} source="H7" label="OFICINA CUSCO" style={{ color: 'purple' }} /> 
                </div>
            ))}
        </MuiGridList>
        
    </div>
);

export const GridList = ({ ids, isLoading, data, currentSort, basePath, rowStyle }) => (
    
    <Table>
        <TableHeader displaySelectAll={false}>
            <TableRow >
                <TableHeaderColumn>DESCRIPCION</TableHeaderColumn>
                <TableHeaderColumn>SALKANTAY</TableHeaderColumn>
                <TableHeaderColumn>LARES</TableHeaderColumn>
                <TableHeaderColumn>EL MERCADO</TableHeaderColumn>
                <TableHeaderColumn>VIÑAK</TableHeaderColumn>
                <TableHeaderColumn>CORPORATIVO</TableHeaderColumn>
                <TableHeaderColumn>HUACAHUASI</TableHeaderColumn>
                <TableHeaderColumn>OFICINA CUSCO</TableHeaderColumn>
            </TableRow>
        </TableHeader>
        <TableBody displayRowCheckbox={false}>
        {ids.map((id) => (
            <TableRow>
                <TableRowColumn><TextField record = {data[id]} source="Descripcion"/></TableRowColumn>
                <TableRowColumn><ColoredNumberField record = {data[id]}  source="Todos" label="Todos los Hoteles" options={{ style: 'currency', currency: 'USD' }} /></TableRowColumn>
                <TableRowColumn><NumberField record = {data[id]} source="H1" label="SALKANTAY" style={{ color: 'purple' }} /></TableRowColumn>
                <TableRowColumn><NumberField record = {data[id]} source="H2" label="LARES" style={{ color: 'purple' }} /> </TableRowColumn>
                <TableRowColumn><NumberField record = {data[id]} source="H3" label="EL MERCADO" style={{ color: 'purple' }} /><NumberField record = {data[id]} source="H2" label="LARES" style={{ color: 'purple' }} /></TableRowColumn>
                <TableRowColumn><NumberField record = {data[id]} source="H4" label="VIÑAK" style={{ color: 'purple' }}/></TableRowColumn>
                <TableRowColumn><NumberField record = {data[id]} source="H5" label="CORPORATIVO" style={{ color: 'purple' }} /> </TableRowColumn>
                <TableRowColumn><NumberField record = {data[id]} source="H6" label="HUACAHUASI" style={{ color: 'purple' }} /> </TableRowColumn>
                <TableRowColumn><NumberField record = {data[id]} source="H7" label="OFICINA CUSCO" style={{ color: 'purple' }} /> </TableRowColumn>
            </TableRow>
            ))}
        </TableBody>
    </Table>
        
    
);
export default GridList;

const columns = [
    {
      Header: "",
      columns: [
        {
          Header: "Descripcion",
          accessor: "descripcion"
        },
        {
          Header: "SALKANTAY",
          id: "H1",
          accessor: d => d.lastName
            }
      ]
    },
    {
      Header: "",
      columns: [
        {
          Header: "TODOS",
          accessor: "todos"
        },
        {
          Header: "LARES",
          accessor: "H2"
        }
      ]
    },
    {
      Header: "",
      columns: [
        {
          Header: "EL MERCADO",
          accessor: "H3"
        },
        {
          Header: "VIÑAK",
          accessor: "H4"
        }
      ]
    },
    {
      Header: "",
      columns: [
        {
          Header: "CORPORATIVO",
          accessor: "H5"
        }
      ]
    },
    {
      Header: "",
      columns: [
        {
          Header: "HUACAHUASI",
          accessor: "H6"
        }
      ]
    },
    {
      Header: "",
      columns: [
        {
          Header: "OFICINA CUSCO",
          accessor: "H7"
        }
      ]
    }
  ];

  export const columns1 = [
    {
      Header: "",
      columns: [
        {
          Header: "Descripcion",
          accessor: "descripcion"
        }
      ]
    }
  ];
  export const data2 = [{
    name: 'VENTAS NETAS',
    age: 26,
    friend: {
      name: 'TERCEROS',
      age: 23,
    }
    }]

 
  export  const columns4 = [{
      Header: 'Descripcion',
      accessor: 'friend.name' // String-based value accessors!
    }, {
      Header: 'Todos'
    },
    {
      Header: 'LARES'
    },
    {
      Header: 'EL MERCADO'
    },
    {
      Header: 'VIÑAK'
    },
    {
      Header: 'CORPORATIVO'
    },
    {
      Header: 'HUACAHUASI'
    },
    { 
        Header: 'OFICINA CUSCO'
    }
  
  ]

  


      const test_columns = [{
        id: 'friendName', // Required because our accessor is not a string
        Header: 'Friend Name',
        accessor: d => d.name // Custom value accessors!
      }, {
        Header: props => <span>Friend Age</span>, // Custom header components!
       
      }]

    export   const test_columns_subgroups1 = [
        {
            Header: 'Name',
            id:'name',
            accessor: data => {
                let output = [];
                _.map(data.friends, friend => {
                    output.push(friend.name);
                });
                return output.join(', ');
            },
        },
    ]

    const test_columns_subgroups = [
      {
          Header: 'Name',
          accessor:'name',
      },
      {
        Header: 'Age',
        accessor:'age',
      },
      {
        Header: 'High',
        accessor:'tall',
      }
    ]
      const test_data = [{
        id: 1,
        name: ' Group Tanner Linsley',
        age: 26,
        friends: [
          {
            name: 'Sub GroupJason Maurer',
            age: 23,
          },
          {
            name: 'Sub Alvaro Medina',
            age: 23,
          }
        ]
        
      },
      {
        id:2,
        name: ' Group Adriana Medina Linsley',
        age: 26,
        friends: [
          {
            name: 'Sub GroupJason Maurer1',
            age: 23,
            tall: 1.55
          },
          {
            name: 'Sub Alvaro Medina1',
            age: 24,
            tall: 2.11
          },
          {
            name: 'Sub Cecilia Fegan1',
            age: 25,
            tall: 2.55
          }
        ]
        
      }]

      JSON.ObjectToArray = function(ids, data)
      {
        var arr = [];
        /*
        for (var key in ids) {
             arr.push(data[key]);
          }*/

          ids.map((id) => (
            arr.push(data[id])
            ))
          return arr;
      };
      
      export const GridListTotalesTest = ({ ids, data, currentSort, basePath, rowStyle }) => (
        <div style={styles.root}>
            <ReactTable
                        data={test_data}
                        columns={test_columns}
                        SubComponent={row => {
                          return (
                            <div style={{ padding: "20px" }}>
                              <em>
                                You can put any component you want here, even another React
                                Table!
                                {console.log(row)
                                }
                              </em>
                              <br />
                              <br />
                              <ReactTable
                                data={row.original.friends}
                                columns={test_columns_subgroups}
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
        </div>
        );




      JSON.ObjectToArray = function(ids, data)
      {
        var arr = [];
        /*
        for (var key in ids) {
             arr.push(data[key]);
          }*/

          ids.map((id) => (
            arr.push(data[id])
            ))
          return arr;
      };
      

      const columnsGroups = [{
        Header: 'Descripcion',
        accessor: 'NombreTipoCuenta' // String-based value accessors!
      }, {
        Header: 'Todos'
      },
      {
        Header: 'SALKANTAY'
      },
      {
        Header: 'LARES'
      },
      {
        Header: 'EL MERCADO'
      },
      {
        Header: 'VIÑAK'
      },
      {
        Header: 'CORPORATIVO'
      },
      {
        Header: 'HUACAHUASI'
      },
      { 
          Header: 'OFICINA CUSCO'
      }
    
    ]

    const columnsSubGroups = [{
      Header: 'Descripcion',
      accessor: "NombreFormatoBalance" // String-based value accessors!
    }, 
    {
      Header: 'Todos'
  
    },
    {
      Header: 'SALKANTAY'
    },
    {
      Header: 'LARES',
    },
    {
      Header: 'EL MERCADO'
    },
    {
      Header: 'VIÑAK'
    },
    {
      Header: 'CORPORATIVO'
    },
    {
      Header: 'HUACAHUASI'
    },
    { 
        Header: 'OFICINA CUSCO'
    }
  
  ]
  
  //<NumberField record = {data[id]} source="H1" label="SALKANTAY" style={{ color: 'purple' }} />

  export const columnsSubGroupsChild = [{
    Header: 'Descripcion',
    accessor: "NombreFormatoGananciasPerdidas" // String-based value accessors!
  }, 
  {
    Header: 'Todos',
    accessor:'Todos',
    Cell: props => <div style={{float:'right'}}><ColoredNumberField record = {{Todos:props.value} }  source="Todos" options={{ style: 'currency', currency: 'USD' }} /></div>
  },
  {
    Header: 'SALKANTAY',
    accessor:'H1',
    Cell: props => <div style={{float:'right'}}><NumberField record = {{H1:props.value} }  source="H1"  options={{ style: 'currency', currency: 'USD'}} /></div>
  },
  {
    Header: 'LARES',
    accessor:'H2',
    Cell: props => <div style={{float:'right'}}><NumberField record = {{H1:props.value} }  source="H1"  options={{ style: 'currency', currency: 'USD' }} /></div>
  },
  {
    Header: 'EL MERCADO',
    accessor:'H3',
    Cell: props => <div style={{float:'right'}}><NumberField record = {{H1:props.value} }  source="H1"  options={{ style: 'currency', currency: 'USD' }} /></div>
  },
  {
    Header: 'VIÑAK',
    accessor:'H4',
    Cell: props => <div style={{float:'right'}}><NumberField record = {{H1:props.value} }  source="H1"  options={{ style: 'currency', currency: 'USD' }} /></div>
  },
  {
    Header: 'CORPORATIVO',
    accessor:'H5',
    Cell: props => <div style={{float:'right'}}><NumberField record = {{H1:props.value} }  source="H1"  options={{ style: 'currency', currency: 'USD' }} /></div>
  },
  {
    Header: 'HUACAHUASI',
    accessor:'H6',
    Cell: props => <div style={{float:'right'}}><NumberField record = {{H1:props.value} }  source="H1"  options={{ style: 'currency', currency: 'USD' }} /></div>
  },
  { 
      Header: 'OFICINA CUSCO',
      accessor:'H7',
      Cell: props => <div style={{float:'right'}}><NumberField record = {{H1:props.value} }  source="H1"  options={{ style: 'currency', currency: 'USD' }} /></div>
  }
  
  ]

  export const GridListGroupTotales = ({data}) => (
    <Table>
        <TableBody displayRowCheckbox={false}>
            <TableRow>
                <TableRowColumn><div style={{float:'right'}}><TextField record = {{Descripcion: 'Totales'}} source="Descripcion"/></div></TableRowColumn>
                <TableRowColumn><div style={{float:'right'}}><ColoredNumberField record = {{col: data.Todos}}  source="col" options={{ style: 'currency', currency: 'USD' }} /></div></TableRowColumn>
                <TableRowColumn><div style={{float:'right'}}><NumberField record = {{col:data.H1} }  source="col"  options={{ style: 'currency', currency: 'USD' }} /></div></TableRowColumn>
                <TableRowColumn><div style={{float:'right'}}><NumberField record = {{col:data.H2} }  source="col"  options={{ style: 'currency', currency: 'USD' }} /></div></TableRowColumn>
                <TableRowColumn><div style={{float:'right'}}><NumberField record = {{col:data.H3} }  source="col"  options={{ style: 'currency', currency: 'USD' }} /></div></TableRowColumn>
                <TableRowColumn><div style={{float:'right'}}><NumberField record = {{col:data.H4} }  source="col"  options={{ style: 'currency', currency: 'USD' }} /></div></TableRowColumn>
                <TableRowColumn><div style={{float:'right'}}><NumberField record = {{col:data.H5} }  source="col"  options={{ style: 'currency', currency: 'USD' }} /></div></TableRowColumn>
                <TableRowColumn><div style={{float:'right'}}><NumberField record = {{col:data.H6} }  source="col"  options={{ style: 'currency', currency: 'USD' }} /></div></TableRowColumn>
                <TableRowColumn><div style={{float:'right'}}><NumberField record = {{col:data.H7} }  source="col"  options={{ style: 'currency', currency: 'USD' }} /></div></TableRowColumn>
            </TableRow>
        </TableBody>
    </Table>
);


      export const  GridListTotales = class Basket extends React.Component {
        
      render() {
            const { ids, data } = this.props;
            const data1 = JSON.ObjectToArray(ids, data);
            /*
            for (var i in data1[0].columns) {
              data1[0].columns[i].Cell  = props => <div style={{float:'right'}}><NumberField record = {{H1:props.value} }  source="H1"  options={{ style: 'currency', currency: 'USD'}} /></div>;
            }
            <Link to={`products/${record.id}`}>{record.reference}</Link>;

            */
            var arrayLength = data1[0].columns.length;
            for (var i = 1; i < arrayLength; i++) {
              data1[0].columns[i].Cell  = props => <div style={{float:'right', fontWeight: 'bold' }}><Link style={{ textDecoration: 'none' }} to={{
                pathname: "/contabilidad",
                search: stringify({ page: 1, perPage: 25, filter: JSON.stringify({ param1: props.value.param1, param2: props.value.param2 }) }),
            }}><NumberField record = {{H1:props.value.value} } elStyle={{ fontSize: '14px' }}  source="H1"  options={{ style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2}} /></Link></div>;
              data1[0]._columns[i].Cell  = props => <div style={{float:'right' }}><Link style={{ textDecoration: 'none' }} to={{
                pathname: "/contabilidad",
                search: stringify({ page: 1, perPage: 25, filter: JSON.stringify({ param1: props.value.param1, param2: props.value.param2 }) }),
            }}><NumberField record = {{H1:props.value.value} } elStyle={{ fontSize: '12px' }}  source="H1"  options={{ style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2}} /></Link></div>;
              data1[0]._columnsGroups[i].Cell  = props => <div style={{float:'right', fontWeight: 'bold' }}><NumberField record = {{H1: props.value} } elStyle={{ fontSize: '12px' }}  source="H1"  options={{ style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2}} /></div>;
              data1[0]._columnsSubGroups[i].Cell  = props => <div style={{float:'right', fontWeight: 'bold' }}><NumberField record = {{H1:props.value} } elStyle={{ fontSize: '12px' }}  source="H1"  options={{ style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2}} /></div>;
            }
            data1[0].columns[0].Cell  = props => <div ><TextField record = {{H1:props.value} } elStyle={{ fontSize: '14px', fontWeight: 'bold' }}  source="H1"  /></div>;
            data1[0].columns[0].width = 210;
            data1[0]._columns[0].Cell  = props => <div ><TextField record = {{H1:props.value} } elStyle={{ fontSize: '12px', fontWeight: 'bold' }}  source="H1"  /></div>;
            data1[0]._columns[0].width = 175;
            data1[0]._columnsGroups[0].Cell  = props => <div ><TextField record = {{H1:props.value} } elStyle={{ fontSize: '12px', fontWeight: 'bold',display: 'flex', flexWrap: 'wrap' }}  source="H1"  /></div>;
            data1[0]._columnsGroups[0].width = 211;
            data1[0]._columnsSubGroups[0].Cell  = props => <div ><TextField record = {{H1:props.value} } elStyle={{ fontSize: '12px', fontWeight: 'bold',display: 'flex', flexWrap: 'wrap' }}  source="H1"  /></div>;
            data1[0]._columnsSubGroups[0].width = 175;

          // console.log(data1);
            return (
              <div style={{ paddingLeft: "10px", paddingRight: "10px" }}>
                    <em>
                          <ReactTable style={{ paddingLeft: "35px" }}
                                    data={data1}
                                    columns={data1[0].columns}
                                    defaultPageSize={1}
                                    showPagination={false}
                                  />
                    </em>
              <ReactTable
                    data={data1[0].groups}
                    defaultPageSize={data1[0].groups.length+1}
                    columns={data1[0]._columnsGroups}
                    SubComponent={row => {
                      return (
                        <div style={{ paddingLeft: "35px" }}>
                          <ReactTable
                            data={row.original.subGroups}
                            columns={data1[0]._columnsSubGroups}
                            defaultPageSize={row.original.subGroups.length}
                            showPagination={false}
                            SubComponent={row => {
                              return (
                                <div style={{ paddingLeft: "35px" }}>
                                  <ReactTable
                                    data={row.original.subGroupChilds}
                                    columns={data1[0]._columns}
                                    defaultPageSize={row.original.subGroupChilds.length}
                                    showPagination={false}
                                    className="-striped -highlight"
                                  />
                                  <br />
                                  <em>
                            
                                  </em>
                                </div>
                              );
                            }}     
                          />
                          
                        </div>
                      );
                    }}
                  />
                            
                          

    </div>
            );
        }
      }
    

      export const GridListTotales2 = ({ ids, data, currentSort, basePath, rowStyle }) => (
    <div style={styles.root}>
        <ReactTable
                    data={JSON.ObjectToArray(ids, data)}
                    columns={columnsGroups}
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
                            data={JSON.ObjectToArray(ids, data)}
                            columns={columnsSubGroups}
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
    </div>
    );

    GridListTotales.defaultProps = {
      data: {},
      ids: [],  };

  export const GridListTotales1  = class GridListTotales extends React.Component {
    constructor() {
      super();
      this.state = {
        data: makeData(5)
      };
    }
    render() {
      const { data } = this.state;
      return (
        <div>
          {console.log(data)}
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
        </div>
      );
    }
  }


