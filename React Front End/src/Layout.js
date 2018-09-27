import { connect } from 'react-redux';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
import {defaultTheme } from 'admin-on-rest';
import LayoutNew from './layout/LayoutNew';

export default connect(state => ({
    theme: state.theme === 'dark' ? darkBaseTheme : defaultTheme,
}))(LayoutNew);
