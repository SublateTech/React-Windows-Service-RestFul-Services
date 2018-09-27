import { Host, Endpoints } from '../config/server';
import { loginActions } from '../config/constants';


/*
    * state props
        - form
        - inProgress
        - error
        - data
*/

export function login(form) {
    return (dispatch) => {
        dispatch(loggingIn(true));
        fetch(Host + Endpoints.auth.login, {
            method: 'POST',
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify(form)
        })
            .then(res => res.json())
            .then(res => { 
                dispatch(loggingIn(false));                
                res.error ? dispatch(loginError(res.error)) : 
                            dispatch(loginSuccess(res.data));
            })
            .catch(err => dispatch(loginError(err)));
    }
}

export function facebookLogin(data) {
    return (dispatch) => {
        dispatch(loggingIn());

        fetch(Host + Endpoints.auth.facebookLogin, {
            method: 'POST',
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify(data)
        })
        .then(res => res.json())
        .then(data => dispatch(loginSuccess(data)))
        .catch(err => dispatch(loginError(err)));
    }
}

export function setUsername(username) {
    return {
        type: loginActions.setUsername,
        username
    }
}

export function setPassword(password) {
    return {
        type: loginActions.setPassword,
        password
    }
}

function loginSuccess(data) {
    return {
        type: loginActions.LoginSuccess,
        data
    }
}

function loginError(error) {
    return {
        type: loginActions.LoginError,
        error
    }
}

function loggingIn(val) {
    return {
        type: loginActions.LoggingIn,
        inProgress: val
    }
}