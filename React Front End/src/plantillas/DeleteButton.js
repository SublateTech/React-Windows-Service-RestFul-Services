import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import FlatButton from 'material-ui/FlatButton';
import { showNotification as showNotificationAction } from 'admin-on-rest';
import { push as pushAction } from 'react-router-redux';

// in src/comments/ApproveButton.js
import { DELETE } from 'admin-on-rest';
import restClient from '../restClientJson';

class DeleteButton extends Component {
    handleClick = () => {
        const { push, record, showNotification } = this.props;
        const updatedRecord = { ...record, is_process: true };
        restClient(DELETE, 'plantillas', { id: record.id, data: updatedRecord })
            .then(() => {
                showNotification('Plantilla eliminada');
                push('/plantillas');
            })
            .catch((e) => {
                console.error(e);
                showNotification('Error: Plantilla no eliminada', 'warning')
            });
    }

    render() {
        return <FlatButton label="Eliminar" onClick={this.handleClick} />;
    }
};

DeleteButton.propTypes = {
    push: PropTypes.func,
    record: PropTypes.object,
    showNotification: PropTypes.func,
};

export default connect(null, {
    showNotification: showNotificationAction,
    push: pushAction,
})(DeleteButton);

