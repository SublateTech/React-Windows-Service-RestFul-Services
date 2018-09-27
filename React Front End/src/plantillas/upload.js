import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import FlatButton from 'material-ui/FlatButton';
import ActionList from 'material-ui/svg-icons/action/list';



export const UploadButton1 = ({
    basePath = '',
    label = 'aor.action.list',
    translate,
}) => (
    <FlatButton
        primary
        label={label }
        icon={<ActionList />}
        containerElement={<Link to={basePath} />}
        style={{ overflow: 'inherit' }}
    />
);

UploadButton1.propTypes = {
    basePath: PropTypes.string,
    label: PropTypes.string,
    translate: PropTypes.func.isRequired,
};


