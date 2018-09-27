// in src/comments/commentSaga.js
import { put, takeEvery, all } from 'redux-saga/effects';
import { push } from 'react-router-redux';
import { showNotification } from 'admin-on-rest';

function* commentApproveSuccess() {
    yield put(showNotification('Comment approved'));
    yield put(push('/comments'));
}

function* commentApproveFailure({ error }) {
    yield put(showNotification('Error: comment not approved', 'warning'));
    console.error(error);
}

export default function* commentSaga() {
    yield all([
        takeEvery('COMMENT_APPROVE_SUCCESS', commentApproveSuccess),
        takeEvery('COMMENT_APPROVE_FAILURE', commentApproveFailure),
    ]);
}