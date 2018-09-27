// in src/comments/ApproveButton.js
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import FlatButton from 'material-ui/FlatButton';
import { stringify } from 'query-string';
import { push as pushAction } from 'react-router-redux';
import FontIcon from 'material-ui/FontIcon';


class ExportButton extends Component {
    render() {

        const {
            resource,
            filterValues,
            type

        } = this.props;

        const apiUrl ='http://localhost:3500';
        const query =  stringify(filterValues)
        const url = `${apiUrl}/${resource}/export/${type}/mayor?${query}`;
        const label = `Exportar a ${type}...` ;
        return <FlatButton  
                secondary={true}
                icon={<FontIcon className="muidocs-icon-custom-github" />} 
                label={label}
                href={url}
                 />;
    }
}

ExportButton.defaultProps = {
    style: { padding: 0 },
};

ExportButton.propTypes = {
    push: PropTypes.func,
    record: PropTypes.object,
    showNotification: PropTypes.func,
};

export default connect(null, {
    push: pushAction,
})(ExportButton);

