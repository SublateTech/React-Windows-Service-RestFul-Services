// in src/comments/ApproveButton.js
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import FlatButton from 'material-ui/FlatButton';
import { exportFormat as exportFormatAction } from './ExcelActions';

class ExcelButton extends Component {
    handleClick = () => {
        
        const { record, basePath, exportFormat, filterValues, sort} = this.props;
        // stored as a property rather than state because we don't want redraw of async updates
        this.params = { pagination: { page: 1, perPage:5 }, sort, filterValues };


        exportFormat(record.id, this.params, basePath);
        // how about push and showNotification?
    }

    render() {
        return <FlatButton secondary={true} label="Export a Excel..." onClick={this.handleClick} />;
    }
}

ExcelButton.propTypes = {
    exportFormat: PropTypes.func,
    record: PropTypes.object,
};

export default connect(null, {
    exportFormat: exportFormatAction,
})(ExcelButton);