import React from 'react';
import { connect } from 'react-redux';
//import { Table, TableBody, TableHeader, TableHeaderColumn, TableRow, TableRowColumn} from 'material-ui/Table';
import Dialog from 'material-ui/Dialog';
//import FlatButton from 'material-ui/FlatButton';
//import RaisedButton from 'material-ui/RaisedButton';
//import DatePicker from 'material-ui/DatePicker';
import SaveButton from './SaveButton';
import SelectField from 'material-ui/SelectField';
import MenuItem from 'material-ui/MenuItem';

//import segments from './data';
import periodos from './data_periodo';

import {setEmpresaId, setPeriodoId} from './EmpresaActions'

const styles = {
  customWidth: {
    width: 400,
  },
};

/**
 * Dialogs can be nested. This example opens a Date Picker from within a Dialog.
 */
class Empresa extends React.Component {
  state = {
    id: '',
    value: 1,
    value_period: 1,
    open: false,
    data: [],
    name: localStorage.getItem('nameEmpresa'),
    empresa: []
    
  };
  
  componentDidMount() {
    
    this.handleOpen();
    fetch('http://localhost:3500/empresas')
    .then(response => response.json())
    .then(data => this.setState({data}))
    this.getNombreEmpresa(localStorage.getItem('empresa'));
  } 

  getNombreEmpresa(id) {
    fetch('http://localhost:3500/empresas/'+ id)
   .then(response => response.json())
   .then(empresa => this.setState({empresa: empresa}))
    
 // localStorage.setItem('nameEmpresa', this.state.empresa.descripcion);
 //console.log(this.state);
 }
  
  handleOpen = () => {
    this.setState({open: true});
  };

  handleClose = () => {
    this.setState({open: false});
    
    localStorage.setItem('nameEmpresa', this.state.empresa.descripcion);
   // console.log(localStorage.getItem('nameEmpresa'));
   localStorage.setItem('empresa', this.state.empresa.id); 
   //this.getNombreEmpresa(localStorage.getItem(this.state.empresa.id));

    this.props.SetEmpresaId(this.state.empresa.id, this.state.empresa.descripcion);
    this.props.SetPeriodoId(this.state.periodo);
    //console.log(this.props);
  };

  handleChange = (event, index, value) => {
     this.setState({id: value}); 
     
     this.getNombreEmpresa(value);
    };
    
  handleChange_period = (event, index, value) => { 
   this.setState({'periodo': value});
   localStorage.setItem('periodo', value); };

  render() {

    const empresas = this.state.data;
    const actions = [
      <SaveButton 
        label="Ok"
        primary={true}
        keyboardFocused={true}
        onClicks={this.handleClose}
      />,
    ];

    //<ViewTitle title='Elija Empresa' />
    /*
    {Object.keys(empresas).map(function(key) {
                         {console.log(empresas[key])}
                         <MenuItem value={empresas[key].descripcion} primaryText={empresas[key].value} />
    */

    return (
      
        <Dialog
          title="Empresas"
          actions={actions}
          modal={false}
          open={this.state.open}
          onRequestClose={this.handleClose}
        >
        
        <SelectField
          floatingLabelText="Empresa"
          value={ localStorage.getItem('empresa') ? localStorage.getItem('empresa') : 0}
          onChange={this.handleChange}
          style={styles.customWidth}
        >
           {
             
             empresas.map(empresa => (
                    <MenuItem value={empresa.id} primaryText={empresa.descripcion} />
                ))}
        </SelectField>
        <br />        
        <SelectField
          floatingLabelText="Periodo"
          value={localStorage.getItem('periodo') ? localStorage.getItem('periodo') : 0}
          onChange={this.handleChange_period}
          autoWidth={true}
        >
           {
             periodos.map(empresa => (
                    <MenuItem value={empresa.id} primaryText={empresa.name} />
                ))}
        </SelectField>
          
          
        </Dialog>
      
    );
  }
}


function mapDispatchToProps(dispatch) {
  return {
    SetEmpresaId: (id, name) => dispatch(setEmpresaId(id, name)),
    SetPeriodoId: (id) => dispatch(setPeriodoId(id)),
    //onIncrementBy: ()=> dispatch(incrementCount({ incrementBy: 5 })),
    //push: pushAction,
  }
}
function mapStateToProps(state) {
  return {
    empresaId: state.empresa.id,
    periodo: state.empresa.periodo,
    name: state.empresa.name,
  }
}


export default connect(mapStateToProps, mapDispatchToProps)(Empresa);