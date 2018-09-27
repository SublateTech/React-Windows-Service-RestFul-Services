import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import {Toolbar, ToolbarGroup, ToolbarSeparator, ToolbarTitle} from 'material-ui/Toolbar';
import compose from 'recompose/compose';
import { push as pushAction } from 'react-router-redux';


//import {setEmpresaId} from './EmpresaActions'
import EmpresaDialog from './EmpresaDialog';

//style={{ position: "fixed" }}
class EmpresaToolbar extends React.Component {

    constructor(props) {
      super(props);
      this.state = {
        empresa: 0,
        ejercicio:  0,
        nombre: '',
        data: [],
      };
    }
  
    componentDidMount1() {
        const { empresaId} = this.props;
         fetch('http://localhost:3500/empresas/'+ empresaId)
        .then(response => response.json())
        .then(data => this.setState({data}))
      } 
      

    handleClick = () => {
        const { push } = this.props;
        console.log(this.props);
        push('/conta_empresa');
        //onSetEmpresaId(999, "Una Empresa");
        }
  
    render() {

        const { periodo, name} = this.props; 
      return (  

    <div >
        <Toolbar style={{backgroundColor: '#00bcd4'}}>
            <ToolbarGroup firstChild={true}>
            
                <ToolbarTitle style = {{fontSize: '14px', fontWeight: 'bold'}} text={ 'Empresa: ' + name}/>
                <ToolbarSeparator />
                <ToolbarTitle style = {{fontSize: '14px', fontWeight: 'bold'}} text={'Ejercicio: ' + periodo}/>
            
                <ToolbarSeparator />
                <EmpresaDialog  color="inherit" label="Cambiar Empresa" primary={true}  />
            </ToolbarGroup>
            
      </Toolbar>
    </div>    
    );
    }
}

function mapDispatchToProps(dispatch) {
    return {
      //onSetEmpresaId: (id, name) => dispatch(setEmpresaId(id, name)),
      //onIncrementBy: ()=> dispatch(incrementCount({ incrementBy: 5 })),
      push: pushAction,
    }
  }
  function mapStateToProps(state) {
    return {
      empresaId: state.empresa.id,
      periodo: state.empresa.periodo,
      name: state.empresa.name,
    }
  }

EmpresaToolbar.propTypes = {
    push: PropTypes.func,
};

const enhance = compose(
    connect(mapStateToProps, mapDispatchToProps)
);

export default enhance(EmpresaToolbar);
