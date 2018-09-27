// in src/comments/ApproveButton.js
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import FlatButton from 'material-ui/FlatButton';
import { stringify } from 'query-string';
import { push as pushAction } from 'react-router-redux';
import FontIcon from 'material-ui/FontIcon';


class PDFButton extends Component {
    render() {

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

        const apiUrl ='http://localhost:3500';
        const query =  stringify(filterValues)
        const url = `${apiUrl}/${resource}/export/pdf/mayor?${query}`;
        console.log(url);
        return <FlatButton  
                secondary={true}
                icon={<FontIcon className="muidocs-icon-custom-github" />} 
                label="Exportar a PDF..." 
                href={url}
                 />;
    }
}

PDFButton.defaultProps = {
    style: { padding: 0 },
};

PDFButton.propTypes = {
    push: PropTypes.func,
    record: PropTypes.object,
    showNotification: PropTypes.func,
};

export default connect(null, {
    push: pushAction,
})(PDFButton);

