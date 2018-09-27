// in src/comments/ApproveButton.js
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { parse, stringify } from 'query-string';
import FlatButton from 'material-ui/FlatButton';
import { showNotification as showNotificationAction } from 'admin-on-rest';
import { push as pushAction } from 'react-router-redux';
import FontIcon from 'material-ui/FontIcon';
import { GET_LIST } from 'admin-on-rest';
import restClient from '../../restClientJson';




class ExcelButton extends Component {
    
    /**
     * Merge list params from 3 different sources:
     *   - the query string
     *   - the params stored in the state (from previous navigation)
     *   - the props passed to the List component
     */
    

    
    handleClick = () => {
        const {
            resource,
            reference,
            data,
            ids,
            children,
            basePath,
            isLoading,
            filterValues

        } = this.props;
        
        /*
        restClient(GET_LIST, 'contabilidad/export', {
            filter: filterValues,
            sort: {},
            pagination: { page: 1, perPage: 10 },
        });
        
        }
        */
        
        restClient('GET_EXPORT', 'contabilidad/export/excel/mayor', {filter: filterValues,
            sort: {},
            pagination: { page: 1, perPage: 10 }})
            .then(() => {
                this.props.showNotification('Archivo Exportado');
                //this.props.push('/plantillas');
            })
            .catch((e) => {
                console.error(e);
                this.props.showNotification('Error: Archivo No Exportado', 'warning')
            });
            
    }

    render() {
        return <FlatButton secondary={true} label="Exportar a Excel..." onClick={this.handleClick} />;
    }
    /*
    render() {
        
        return <FlatButton  
                secondary={true}
                icon={<FontIcon className="muidocs-icon-custom-github" />} 
                label="Exportar a Excel..." 
                href="http://localhost:3500/contabilidad/export/excel/mayor"
                 />;
    }
    */
}


ExcelButton.defaultProps = {
    style: { padding: 0 },
};

ExcelButton.propTypes = {
    push: PropTypes.func,
    record: PropTypes.object,
    showNotification: PropTypes.func,
    
};

export default connect(null, {
    showNotification: showNotificationAction,
    push: pushAction,
})(ExcelButton);