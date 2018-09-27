import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import IconButton from 'material-ui/IconButton';
import {cyan500} from 'material-ui/styles/colors';
import ContentCreate from 'material-ui/svg-icons/content/create';
import FlatButton from 'material-ui/FlatButton';
import FontIcon from 'material-ui/FontIcon';
import ActionAndroid from 'material-ui/svg-icons/action/android';
import {fullWhite} from 'material-ui/styles/colors';
const style = {
    margin: 12,
  };

const ExcelButton = ({ basePath = '', record = {} }) => (
    <FlatButton
      label="Export to Excel.."
      secondary={true}
      containerElement={<Link to={`${basePath}/${record.id}`} />}
      icon={<FontIcon className="muidocs-icon-custom-github" />}
    />
);

ExcelButton.propTypes = {
    basePath: PropTypes.string,
    record: PropTypes.object,
};

ExcelButton.defaultProps = {
    style: { padding: 0 },
};

export default ExcelButton;
