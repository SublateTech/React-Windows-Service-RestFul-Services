import {
    TOGGLE_SIDEBAR,
    SET_SIDEBAR_VISIBILITY,
    REFRESH_VIEW,
    SET_EMPRESA_ID,
    SET_PERIODO_ID
  } from './EmpresaActions';
  
  const defaultState = {
    sidebarOpen: false,
    viewVersion: 123,
    id: localStorage.getItem('empresa') ? localStorage.getItem('empresa') : 0,
    periodo: localStorage.getItem('periodo') ? localStorage.getItem('periodo') : 0,
    name: localStorage.getItem('nameEmpresa') ? localStorage.getItem('nameEmpresa') : ''
  };

export default (previousState = defaultState, { type, payload }) => {
    //console.log(defaultState);
    switch (type) {
        case SET_EMPRESA_ID:
           // console.log(payload.id);
            return {
                ...previousState,
                id: payload.id,
                name: payload.name,
            };
        case SET_PERIODO_ID:
            //console.log(payload.id);
            return {
                ...previousState,
                periodo: payload.id,
            };
        case TOGGLE_SIDEBAR:
            return {
                ...previousState,
                sidebarOpen: !previousState.sidebarOpen,
            };
        case SET_SIDEBAR_VISIBILITY:
            return { ...previousState, sidebarOpen: payload };
        case REFRESH_VIEW:
            return {
                ...previousState,
                viewVersion: previousState.viewVersion + 1,
            };
        default:
            return previousState;
    }
  };