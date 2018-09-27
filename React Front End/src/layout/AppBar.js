import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import MuiAppBar from 'material-ui/AppBar';
import muiThemeable from 'material-ui/styles/muiThemeable';
//import Button from 'material-ui/Button';
//import IconMenu from 'material-ui/IconMenu';
//import IconButton from 'material-ui/IconButton';
//import FontIcon from 'material-ui/FontIcon';
//import NavigationExpandMoreIcon from 'material-ui/svg-icons/navigation/expand-more';
//import MenuItem from 'material-ui/MenuItem';
//import DropDownMenu from 'material-ui/DropDownMenu';
//import RaisedButton from 'material-ui/RaisedButton';
//, ToolbarSeparator, ToolbarTitle
//import {ToolbarTitle} from 'material-ui/Toolbar';
import compose from 'recompose/compose';
import { toggleSidebar as toggleSidebarAction } from './uiActions';
import { push as pushAction } from 'react-router-redux';

import EmpresaToolbar from '../empresa/EmpresaToolbar';
//style={{ position: "fixed" }}
class AppBar extends React.Component {

    render() {
        const { title, toggleSidebar } = this.props;
        //console.log(this.state.data);
      return (  

    <div style={{ paddingTop: 56 }} >
        <MuiAppBar title={title} onLeftIconButtonTouchTap={toggleSidebar} style={{position: 'fixed', top: 0 }}> 
        <EmpresaToolbar />
        </MuiAppBar>
    </div>    
    );
    }
}
/*
            <DropDownMenu value={this.state.value_moneda} onChange={this.handleChange_moneda}>
                <MenuItem value={1} primaryText="2017" />
                <MenuItem value={2} primaryText="2018" />
            </DropDownMenu>
            <DropDownMenu value={this.state.value} onChange={this.handleChange}>
                <MenuItem value={1} primaryText="Empresa 1" />
                <MenuItem value={2} primaryText="Empresa 2" />
            </DropDownMenu>
            <ToolbarGroup>
            <ToolbarTitle text="Options" />
            <FontIcon className="muidocs-icon-custom-sort" />
            <ToolbarSeparator />
            <RaisedButton label="Create Broadcast" primary={true} />
            <IconMenu
                iconButtonElement={
                <IconButton touch={true}>
                    <NavigationExpandMoreIcon />
                </IconButton>
                }
            >
                <MenuItem primaryText="Download" />
                <MenuItem primaryText="More Info" />
            </IconMenu>
            </ToolbarGroup>

*/


AppBar.propTypes = {
    title: PropTypes.oneOfType([PropTypes.string, PropTypes.element])
        .isRequired,
    toggleSidebar: PropTypes.func.isRequired,
    push: PropTypes.func,
};

const enhance = compose(
    muiThemeable(), // force redraw on theme change
    connect(null, {
        push: pushAction,
        toggleSidebar: toggleSidebarAction,
    })
);

export default enhance(AppBar);
