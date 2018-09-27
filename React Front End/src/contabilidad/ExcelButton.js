// in src/comments/ApproveButton.js
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import FlatButton from 'material-ui/FlatButton';

import { push as pushAction } from 'react-router-redux';
import FontIcon from 'material-ui/FontIcon';


class ExcelButton extends Component {
    OnClicks = () => {
        this.props.onClicks(this.props.value);
      }
    
    render() {
        return <FlatButton  
                secondary={true}
                icon={<FontIcon className="muidocs-icon-custom-github" />} 
                label="Exportar a Excel..." 
                href="http://localhost:3500/contabilidad/export/excel/mayor"
                 />;
    }
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
    push: pushAction,
})(ExcelButton);


