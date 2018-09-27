// in src/comment/commentActions.js
import { GET_LIST } from 'admin-on-rest';
export const EXPORT_FORMAT = 'EXPORT_FORMAT';
export const exportFormat = (params, basePath) => ({
    type: EXPORT_FORMAT,
    payload: { id, data: { ...data, is_excel: true, is_pdf: false } },
    meta: { resource: basePath, fetch: GET_LIST, cancelPrevious: false },
});