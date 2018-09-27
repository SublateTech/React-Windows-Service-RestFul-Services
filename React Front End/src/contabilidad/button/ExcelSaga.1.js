// in src/comments/commentSaga.js
import { put, takeEvery, all } from 'redux-saga/effects';
import { push } from 'react-router-redux';
import { showNotification } from 'admin-on-rest';

function* exportFormatSuccess() {
    yield put(showNotification('Excel exported'));
    yield put(push('/contabilidad'));
}

function* exportFormatFailure({ error }) {
    yield put(showNotification('Error: export not exported', 'warning'));
    console.error(error);
}

export default function* exportFormatSaga() {
    yield all([
        takeEvery('EXPORT_FORMAT_SUCCESS', exportFormatSuccess),
        takeEvery('EXPORT_FORMAT_FAILURE', exportFormatFailure),
    ]);
}