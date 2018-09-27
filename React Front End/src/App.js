import 'babel-polyfill';
import React, { Component } from 'react';
import { Admin, Delete, Resource } from 'admin-on-rest'; //simpleRestClient,

import './App.css';

import authClient from './authClient';
import sagas from './sagas';
import themeReducer from './themeReducer';
import Login from './Login';
import LayoutNew from './Layout';
import Menu from './Menu';
import { Dashboard } from './dashboard';
import customRoutes from './routes';
import translations from './i18n';

import { VisitorList, VisitorEdit, VisitorDelete, VisitorIcon } from './visitors';
import { CommandList, CommandEdit, CommandIcon } from './commands';
import { ProductList, ProductCreate, ProductEdit, ProductIcon } from './products';
import { CategoryList, CategoryEdit, CategoryIcon } from './categories';
import { ReviewList, ReviewEdit, ReviewIcon } from './reviews';
import { TipoPlantillaList, PlantillasList, CreatePlantilla, EditPlantilla, DeletePlantilla } from './plantillas';
import { ContaList } from './contabilidad';

import restClientJson from './restClientJson';
//import restClient from './restClient';
//import fakeRestServer from './restServer';

import UserIcon from 'material-ui/svg-icons/social/group';

//import exportFormatSaga from './contabilidad/button/ExcelSaga';
//import rate from './bitcoin/rateReducer';
import empresaReducer from './empresa/EmpresaReducer';

class App extends Component {
    componentWillMount() {
        //this.restoreFetch = fakeRestServer();
    }

    componentWillUnmount() {
        this.restoreFetch();
    }

    render() {
        return (
            <Admin
                title="Sistemas Shiol"
                restClient={restClientJson}
                customReducers={{ theme: themeReducer, empresa: empresaReducer }}
                customSagas={sagas}
                customRoutes={customRoutes}
                authClient={authClient}
                dashboard={Dashboard}
                loginPage={Login}
                appLayout={LayoutNew}
                menu={Menu}
                messages={translations}
                locale='sp'
            >
                <Resource name="plantillas" list={PlantillasList}  create={CreatePlantilla}  edit={EditPlantilla} icon={UserIcon} remove={DeletePlantilla} />
                <Resource name="contabilidad" list={ContaList}  />
                <Resource name="customers" list={VisitorList} edit={VisitorEdit} remove={VisitorDelete} icon={VisitorIcon} />
                <Resource name="commands" list={CommandList} edit={CommandEdit} remove={Delete} icon={CommandIcon} options={{ label: 'Orders' }}/>
                <Resource name="products" list={ProductList} create={ProductCreate} edit={ProductEdit} remove={Delete} icon={ProductIcon} />
                <Resource name="categories" list={CategoryList} edit={CategoryEdit} remove={Delete} icon={CategoryIcon} />
                <Resource name="files" />
                <Resource name="hoteles" />
                <Resource name="TipoEstadoFinanciero"/>
                <Resource name="reviews" list={ReviewList} edit={ReviewEdit} icon={ReviewIcon} />
                <Resource name="tipo_plantillas" list={TipoPlantillaList} />
                

            </Admin>
        );
    }
}

export default App;
