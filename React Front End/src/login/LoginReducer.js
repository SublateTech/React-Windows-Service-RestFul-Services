import { loginActions } from '../config/constants';

const initialState = {
  form: {
    username: '',
    password: ''
  },
  data: null,
  inProgress: false,
  error: null
};

export default function loginReducer(state = initialState, action) {
    switch(action.type) {
        case loginActions.LoggingIn: 
            return {
                ...state,
                inProgress: action.inProgress
            }            
        case loginActions.LoginError:
             return {
                ...state, 
                error: action.error,
            }

        case loginActions.LoginSuccess:
            return {
                ...state,
                inProgress: false,
                error: null,
                data: action.data
            }
        case loginActions.setUsername:
            return {
                ...state,
                form: { 
                    username: action.username,
                    password: state.form.password
                }
            }
        case loginActions.setPassword:
            return {
                ...state,
                form: { 
                    username: state.form.username,
                    password: action.password
                }
            }
        default: 
            return {
                ...state
            }
    }
}