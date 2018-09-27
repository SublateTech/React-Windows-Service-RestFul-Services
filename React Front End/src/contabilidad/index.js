import React from 'react';
import {
    List,
    translate,
    Datagrid,
    DateField,
    DateInput,
    Delete,
    Edit,
    Filter,
    FormTab,
    LongTextInput,
    NullableBooleanInput,
    NumberField,
    ReferenceManyField,
    TabbedForm,
    TextField,
    TextInput,
    ReferenceInput,
    SelectInput,
    RefreshButton, 
} from 'admin-on-rest';
import Icon from 'material-ui/svg-icons/social/person';

import EditButton from '../buttons/EditButton';
import NbItemsField from '../commands/NbItemsField';
import ProductReferenceField from '../products/ProductReferenceField';
import StarRatingField from '../reviews/StarRatingField';
import FullNameField from './FullNameField';
import { CardActions } from 'material-ui/Card';
//import List from '../mui/list/List';

//import ApproveButton from './ApproveButton';
//import PDFButton from './PDFButton';
import ExportButton from './button/ExportButton';

//import ApproveButton from '../buttons/ApproveButton';

import {GridList, GridListTotales } from './GridList';

//import {Basket,SubComponents, SubComponent10K} from './Basket';
//import BitCoinRate from '../bitcoin/BitCoinRate';
  
import { Card } from 'material-ui';
  
export const VisitorIcon = Icon;

const cardActionStyle = {
    zIndex: 2,
    display: 'inline-block',
    float: 'left',
};

const ContaActions = ({ resource, filters, displayedFilters, filterValues, basePath, showFilter, sort }) => (
    
    <CardActions style={cardActionStyle}>
        <ExportButton type='excel' resource = {resource} basePath={basePath} record = {{id: '111'}} filterValues = {filterValues} sort={sort}/>
        <ExportButton type='pdf' resource = {resource} basePath={basePath} record = {{id: '111'}} filterValues = {filterValues} sort={sort} />
        {filters && React.cloneElement(filters, { resource, showFilter, displayedFilters, filterValues, context: 'button' }) }
        <RefreshButton />
        
    </CardActions>
);

const ContaFilter = (props) => (
    <Filter {...props}>

        <SelectInput alwaysOn source="moneda" choices={[
            { id: 'USD', name: 'Dólares' },
            { id: 'PEN', name: 'Soles' },

            ]} />

        <ReferenceInput label="Hotel" source="HotelID" reference="hoteles">
            <SelectInput optionText="descripcion" />
        </ReferenceInput>
        <ReferenceInput label="Tipo Estado Financiero" source="TipoFormatoGGPPID" reference="TipoEstadoFinanciero">
            <SelectInput optionText="descripcion" />
        </ReferenceInput>

        <SelectInput alwaysOn label="Mes" source="mes" choices={[
            { id: '0', name: 'Apertura' },
            { id: '1', name: 'Enero' },
            { id: '2', name: 'Febrero' },
            { id: '3', name: 'Marzo' },
            { id: '4', name: 'Abril' },
            { id: '5', name: 'Mayo' },
            { id: '6', name: 'Junio' },
            { id: '7', name: 'Julio' },
            { id: '8', name: 'Agosto' },
            { id: '9', name: 'Setiembre' },
            { id: '10', name: 'Octubre' },
            { id: '11', name: 'Noviembre' },
            { id: '12', name: 'Diciembre' },
            { id: '13', name: 'Cierre' }
            ]} />

        <SelectInput alwaysOn label="Tipo" source="tipo" choices={[
            { id: 'mensual', name: 'Mensual' },
            { id: 'acumulado', name: 'Acumulado' },
            ]} />

    </Filter>
);

const colored = WrappedComponent => props => props.record[props.source] > 500 ?
    <span style={{ color: 'red' }}><WrappedComponent {...props} /></span> :
    <WrappedComponent {...props} />;

const ColoredNumberField = colored(NumberField);
ColoredNumberField.defaultProps = NumberField.defaultProps;




export const ContaList = (props) => (
    
    <List {...props} title = "Reporte Mayorizado" {...props} actions={<ContaActions/>} filters={<ContaFilter />} sort={{ field: 'id', order: 'ASC' }} perPage={25}>
        <GridListTotales {...props} record={1}/>
    </List>
    
);

export const ContaList3 = (props) => (
        <List {...props} filters={<ContaFilter />} sort={{ field: 'id', order: 'ASC' }} perPage={25}>
                <Datagrid bodyOptions={{ stripedRows: true, showRowHover: true }}>
                    <TextField  source="id" label="id" style={{ color: 'purple' }} />
                    <TextField  source="Descripcion" label="Descripcion" style={{ color: 'purple' }} />
                    <ReferenceManyField label="Cuenta" reference="contabilidad" target="group_id">
                            
                                <Datagrid bodyOptions={{ stripedRows: true, showRowHover: true }}>
                                    <TextField  source="id" label="id" style={{ color: 'purple' }} />
                                    <TextField  source="Descripcion" label="Descripcion" style={{ color: 'purple' }} />
                                    <ReferenceManyField label="Cuenta" reference="contabilidad" target="group_id">
                                            <GridList />            
                                    </ReferenceManyField>
                                </Datagrid>
                            
                    </ReferenceManyField>
                </Datagrid>
        </List>
);

export const ContaList2 = (props) => (
    <Card>
        <Card>
        <List {...props} filters={<ContaFilter />} sort={{ field: 'id', order: 'ASC' }} perPage={25}>
                <Datagrid bodyOptions={{ stripedRows: true, showRowHover: true }}>
                    <TextField  source="Descripcion"/>
                    <ColoredNumberField  source="Todos" label="Todos los Hoteles" options={{ style: 'currency', currency: 'USD' }} />   
                    <NumberField  source="H1" label="SALKANTAY" style={{ color: 'purple' }} />
                    <NumberField  source="H2" label="LARES" style={{ color: 'purple' }} />
                    <NumberField  source="H3" label="EL MERCADO" style={{ color: 'purple' }} />
                    <NumberField  source="H4" label="VIÑAK" style={{ color: 'purple' }} /> 
                    <NumberField  source="H5" label="CORPORATIVO" style={{ color: 'purple' }} />
                    <NumberField  source="H6" label="HUACAHUASI" style={{ color: 'purple' }} /> 
                    <NumberField  source="H7" label="OFICINA CUSCO" style={{ color: 'purple' }} />
                </Datagrid>
        </List>
        </Card>
        <Card>
        <List {...props} filters={<ContaFilter />} sort={{ field: 'id', order: 'ASC' }} perPage={25}>
            <GridList />
    </List>
        </Card>
    </Card>
    
);

const VisitorTitle = ({ record }) => record ? <FullNameField record={record} size={32} /> : null;

export const ContaEdit = (props) => (
    <Edit title={<VisitorTitle />} {...props}>
        <TabbedForm>
            <FormTab label="resources.customers.tabs.identity">
                <TextInput source="first_name" style={{ display: 'inline-block' }} />
                <TextInput source="last_name" style={{ display: 'inline-block', marginLeft: 32 }} />
                <TextInput type="email" source="email" validation={{ email: true }} options={{ fullWidth: true }} style={{ width: 544 }} />
                <DateInput source="birthday" />
            </FormTab>
            <FormTab label="resources.customers.tabs.address">
                <LongTextInput source="address" style={{ maxWidth: 544 }} />
                <TextInput source="zipcode" style={{ display: 'inline-block' }} />
                <TextInput source="city" style={{ display: 'inline-block', marginLeft: 32 }} />
            </FormTab>
            <FormTab label="resources.customers.tabs.orders">
                <ReferenceManyField addLabel={false} reference="commands" target="customer_id">
                    <Datagrid>
                        <DateField source="date" />
                        <TextField source="reference" />
                        <NbItemsField />
                        <NumberField source="total" options={{ style: 'currency', currency: 'USD' }} />
                        <TextField source="status" />
                        <EditButton />
                    </Datagrid>
                </ReferenceManyField>
            </FormTab>
            <FormTab label="resources.customers.tabs.reviews">
                <ReferenceManyField addLabel={false} reference="reviews" target="customer_id">
                    <Datagrid filter={{ status: 'approved' }}>
                        <DateField source="date" />
                        <ProductReferenceField />
                        <StarRatingField />
                        <TextField source="comment" style={{ maxWidth: '20em', overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }} />
                        <EditButton style={{ padding: 0 }} />
                    </Datagrid>
                </ReferenceManyField>
            </FormTab>
            <FormTab label="resources.customers.tabs.stats">
                <NullableBooleanInput source="has_newsletter" />
                <DateField source="first_seen" style={{ width: 128, display: 'inline-block' }} />
                <DateField source="latest_purchase" style={{ width: 128, display: 'inline-block' }} />
                <DateField source="last_seen" style={{ width: 128, display: 'inline-block' }} />
            </FormTab>
        </TabbedForm>
    </Edit>
);

const VisitorDeleteTitle = translate(({ record, translate }) => <span>
    {translate('resources.customers.page.delete')}&nbsp;
    {record && <img src={`${record.avatar}?size=25x25`} width="25" alt="" />}
    {record && `${record.first_name} ${record.last_name}`}
</span>);

export const ContaDelete = (props) => <Delete {...props} title={<VisitorDeleteTitle />} />;
