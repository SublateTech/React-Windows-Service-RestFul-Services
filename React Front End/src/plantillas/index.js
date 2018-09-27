// in src/posts.js
import React from 'react';
import { DateInput, Filter, FileInput, FileField, List, Edit, Create, Delete, Datagrid, TextField, DisabledInput,  ReferenceInput, required, SelectInput, SimpleForm } from 'admin-on-rest';

//import FlatButton from 'material-ui/FlatButton';

import ProcessButton from './ProcessButton';
import DeleteButton from '../buttons/DeleteButton';


export const TipoPlantillaList = (props) => (
    <List  {...props}>
        <Datagrid>
            <TextField source="id" />
            <TextField source="tipo"  />
            <TextField source="tipo_nombre" />
            
        </Datagrid>
    </List>
);


const ContaFilter = (props) => (
    <Filter {...props}>
      
        <ReferenceInput label="Tipo Plantilla" source="tipo" reference="tipo_plantillas" validate={required} allowEmpty>
                <SelectInput optionText="tipo_nombre" />
            </ReferenceInput>

        <SelectInput alwaysOn label="Estado" source="estado" choices={[
            { id: '-1', name: 'Todos' },
            { id: '0', name: 'No Procesado' },
            { id: '1', name: 'En Proceso' },
            { id: '2', name: 'Procesado' },
            ]} />

        <DateInput source="fecha_creacion" label='Fecha de Creación' />

    </Filter>
);

//<EditButton label=''/>
export const PlantillasList = (props) => (
    <List  {...props} filters={<ContaFilter />} perPage={10}>
        <Datagrid>
            
            <DeleteButton  />
            <ProcessButton />

            <TextField source="tipo_nombre" label='Plantilla'  />
            <TextField source="fecha_creacion" label='F Creación'/>
            <TextField source="fecha_proceso" label='F Proceso'/>
            <TextField source="archivo" />
            <TextField source="estado" />
        </Datagrid>
    </List>
);

// <TextInput source="descripcion" />
export const CreatePlantilla = (props) => (
    <Create {...props}>
        <SimpleForm redirect="list">
            <ReferenceInput label="Tipo Plantilla" source="tipo" reference="tipo_plantillas" validate={required} allowEmpty>
                <SelectInput optionText="tipo_nombre" />
            </ReferenceInput>
            <FileInput source="files" multiple='true' labelMultiple='Haga click aquí para pegar archivos excel' label="Related files" accept=".xls, .xlsx">
                <FileField source="src" title="title" />
            </FileInput>
        </SimpleForm>
    </Create>
);

const PlantillaTitle = ({ record }) => {
    return <span>Post {record ? `"${record.title}"` : ''}</span>;
};

export const EditPlantilla = (props) => (
    <Edit title={<PlantillaTitle />} {...props}>
        <SimpleForm>
            <DisabledInput source="id" />
            <ReferenceInput label="Tipo Plantilla" source="tipo" reference="tipo_plantillas" allowEmpty>
                <SelectInput optionText="tipo_nombre" />
            </ReferenceInput>
            <FileInput source="files" label="Related files" accept=".xls, .xlsx">
                <FileField source="src" title="title" />
            </FileInput>
        </SimpleForm>
    </Edit>
);




export const DeletePlantilla = (props) => <Delete {...props}  />;
