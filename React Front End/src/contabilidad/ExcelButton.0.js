// in src/comments/ApproveButton.js
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { parse, stringify } from 'query-string';
import FlatButton from 'material-ui/FlatButton';
import { showNotification as showNotificationAction } from 'admin-on-rest';
import { push as pushAction } from 'react-router-redux';
import FontIcon from 'material-ui/FontIcon';
import { GET_MANY_REFERENCE } from 'admin-on-rest';
import restClient from '../restClientJson';
import {
    getIds,
    getReferences,
    nameRelatedTo,
} from 'admin-on-rest';

import {
    crudGetManyReference as crudGetManyReferenceAction,
    
} from 'admin-on-rest';

import { createSelector } from 'reselect';


class ExcelButton extends Component {
    
    constructor(props) {
        super(props);
    }

    componentDidMount() {
      //  this.updateData();
      console.log(this.props);
        if (Object.keys(this.props.query).length > 0) {
           // this.props.changeListParams(this.props.resource, this.props.query);
            console.log(this.props.query);
        }
    }
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

        } = this.props;
        //const { filters, showNotification } = this.props;
        console.log(this.props);
        /*
        restClient(GET_MANY_REFERENCE, 'contabilidad/export/excel/mayor', {})
            .then(() => {
                showNotification('Plantilla eliminada');
                push('/plantillas');
            })
            .catch((e) => {
                console.error(e);
                showNotification('Error: Plantilla no eliminada', 'warning')
            });
            */
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

export default connect(mapStateToProps, {
    showNotification: showNotificationAction,
    push: pushAction,
})(ExcelButton);