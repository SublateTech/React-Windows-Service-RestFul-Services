// in src/comments/ApproveButton.js
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import FlatButton from 'material-ui/FlatButton';
//import { showNotification as showNotificationAction } from 'admin-on-rest';
import { push as pushAction } from 'react-router-redux';

class SaveButton extends Component {
    OnClicks = () => {
        this.props.onClicks(this.props.value);
      }
    
    
    handleClick = () => {
       // const { push} = this.props;
      /*  
        fetch('/empresas', { method: 'GET'})
            .then(() => {
                //showNotification('Comment approved');
                this.OnClicks();
                push('/contabilidad');
            })
            .catch((e) => {
                console.error(e);
                showNotification('Error: comment not approved', 'warning')
            });
            */
         //  push('/contabilidad');
           this.OnClicks();
           
    }

    render() {
        return <FlatButton label="OK" onClick={this.handleClick} />;
    }
}

SaveButton.propTypes = {
    push: PropTypes.func,
    record: PropTypes.object,
    showNotification: PropTypes.func,
};

export default connect(null, {
  //  showNotification: showNotificationAction,
    push: pushAction,
})(SaveButton);