//iuActions
export const TOGGLE_SIDEBAR = 'AOR/TOGGLE_SIDEBAR';

export const toggleSidebar = () => ({
    type: TOGGLE_SIDEBAR,
});

export const SET_SIDEBAR_VISIBILITY = 'AOR/SET_SIDEBAR_VISIBILITY';
export const SET_EMPRESA_ID = 'AOR/SET_EMPRESA_ID';
export const SET_PERIODO_ID = 'AOR/SET_PERIODO_ID';

export const setEmpresaId = (id = 0, name = '') => ({
    type: SET_EMPRESA_ID,
    payload: {id: id, name: name},
});

export const setPeriodoId = (id) => ({
    type: SET_PERIODO_ID,
    payload: {id: id},
});

// SET_START_DATE
export const setStartDate = (startDate) => ({
    type: 'SET_START_DATE',
    startDate
  });

export const REFRESH_VIEW = 'AOR/REFRESH_VIEW';

export const refreshView = () => ({
    type: REFRESH_VIEW,
});