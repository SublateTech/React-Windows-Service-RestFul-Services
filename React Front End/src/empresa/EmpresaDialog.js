import React from 'react';
import { connect } from 'react-redux';
//import { Table, TableBody, TableHeader, TableHeaderColumn, TableRow, TableRowColumn} from 'material-ui/Table';
import Dialog from 'material-ui/Dialog';
//import FlatButton from 'material-ui/FlatButton';
import RaisedButton from 'material-ui/RaisedButton';
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
class EmpresaDialog extends React.Component {
  constructor(props) {
    super(props)
    this.handleKeyPress = this.handleKeyPress.bind(this)
    this.escFunction = this.escFunction.bind(this);
  }

  state = {
    id: '',
    value: 1,
    value_period: 1,
    open: false,
    data: [],
    periodo: localStorage.getItem('periodo'),
    empresa: {
      id: localStorage.getItem('empresa'),
      name: localStorage.getItem('nameEmpresa'),
    },
    keyScape: false,
  }

  escFunction(event){
    if(event.keyCode === 27) {
      //Do whatever when esc is pressed
      this.handleOpen();
      //console.log(event.keyCode);
      this.setState({keyScape: true})
    }
    
  }
  
  componentWillUnmount(){
    document.removeEventListener("keydown", this.escFunction, false);
  }

  componentDidMount() {
    fetch('http://localhost:3500/empresas')
    .then(response => response.json())
    .then(data => this.setState({data}))
    this.getNombreEmpresa(this.state.empresa.id);
    document.addEventListener("keydown", this.escFunction, false);
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
    if (!this.state.keyScape)
    {
        localStorage.setItem('nameEmpresa', this.state.empresa.descripcion);
        localStorage.setItem('empresa', this.state.empresa.id); 
        localStorage.setItem('periodo', this.state.periodo); 
      
        this.props.SetEmpresaId(this.state.empresa.id, this.state.empresa.descripcion);
        this.props.SetPeriodoId(this.state.periodo);
    }
    this.setState({keyScape: false})
  };

  handleChange = (event, index, value) => {
     this.setState({id: value}); 
     this.getNombreEmpresa(value);
    };
    
  handleChange_period = (event, index, value) => { 
   this.setState({'periodo': value});
  };

  handleKeyPress = (e) => {
    if (e.key === 'Enter') {
      console.log('do validate');
    }
    console.log(e.key);

  }

  render() {

    const empresas = this.state.data;
    const actions = [
      <SaveButton 
        label="Ok"
        primary={true}
        keyboardFocused={true}
        onClicks={this.handleClose}
        onKeyPress={this.handleKeyPress}
      />,
    ];

    return (
      <div>
        <RaisedButton label="Cambiar Empresa" onClick={this.handleOpen} onKeyPress={this.handleKeyPress} primary={true} />
        <Dialog
          title="Empresas"
          actions={actions}
          modal={false}
          open={this.state.open}
          onRequestClose={this.handleClose}
          onKeyPress={this.handleKeyPress}
        >
        <SelectField
          floatingLabelText="Empresa"
          value={ this.state.empresa.id}
          onChange={this.handleChange}
          style={styles.customWidth}>
           {            
             empresas.map(empresa => (
                    <MenuItem key={empresa.id} value={empresa.id} primaryText={empresa.descripcion} />
                ))}
        </SelectField>
        <br />        
        <SelectField
          floatingLabelText="Periodo"
          value={this.state.periodo}
          onChange={this.handleChange_period}
          autoWidth={true} >
           {
             periodos.map(periodo => (
                    <MenuItem key={periodo.id} value={periodo.id} primaryText={periodo.name} />
                ))}
        </SelectField>
        </Dialog>
        </div>
    );
  }
}


function mapDispatchToProps(dispatch) {
  return {
    SetEmpresaId: (id, name) => dispatch(setEmpresaId(id, name)),
    SetPeriodoId: (id) => dispatch(setPeriodoId(id)),
  }
}
function mapStateToProps(state) {
  return {
    empresaId: state.empresa.id,
    periodo: state.empresa.periodo,
    name: state.empresa.name,
  }
}


export default connect(mapStateToProps, mapDispatchToProps)(EmpresaDialog);