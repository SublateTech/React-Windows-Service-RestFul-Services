import  jsonServerRestClient  from './rest/jsonServer';

const restClient = jsonServerRestClient('http://localhost:3500');
export default (type, resource, params) => new Promise(resolve => setTimeout(() => resolve(restClient(type, resource, params)), 500));
