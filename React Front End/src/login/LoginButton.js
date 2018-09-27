
render() {
    return (<TouchableOpacity style={Style.btnSubmit} onPress={this.onLoginPressed.bind(this)}>
             <Text style={Style.btnText}>Login</Text>
           </TouchableOpacity>)
}

onLoginPressed() {
    const { username, password } = this.props.form;
    this.props.login({ username, password}); // the login is using the fetch api

    // props are not updated with the new state values
    console.log(this.props)
}


function mapStateToProps(state) {
    return {
        ...state.login
    }
}

function mapDispatchToProps(dispatch) {
    return {
        login: (formData) => dispatch(login(formData)),
        facebookLogin: (formData) => dispatch(facebookLogin(formData)),
        setUsername: (username) => dispatch(setUsername(username)),
        setPassword: (password) => dispatch(setPassword(password)),        
    }
}

/*
const actions = {
    login,
    facebookLogin,
    setUsername,
    setPassword
};

export default connect(mapState, actions)(Login);
*/
/*
Also, you're making unnecessary copies. You definitely don't want to do return {...state} in the reducer default case - 
you should do return state to return the existing object. Also, you don't need to do return {...state.login} in the
mapState function. That probably won't break anything, but you might as well do return state.login just to avoid the unnecessary copy.
*/

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Login);