import React, { useEffect, useState } from 'react';
import { DataGrid, GridPaginationModel } from '@mui/x-data-grid';
import { ApiClient } from '../api/api-client';
import { CreateCustomerDto, CustomerApi, CustomerDto } from '../api/CustomersApi';
import Button from '@mui/material/Button';
import { Dialog, DialogActions, DialogContent, DialogTitle } from '@mui/material';
import TextField from '@mui/material/TextField';

export default function CustomerDataGrid() {
    const [customers, setCustomers] = useState<CustomerDto[]>([]);
    const [totalRows, setTotalRows] = useState<number>(0);
    const [paginationModel, setPaginationModel] = useState<GridPaginationModel>({
        page: 0,
        pageSize: 10
    });
    const [openModal, setOpenModal] = useState<boolean>(false);
    
    const [newCustomer, setNewCustomer] = useState<CreateCustomerDto>({
        name: '',
        surname: '',
        phone: '',
    });
    
    useEffect(() => {
        const fetchCustomers = async () => {
            const { customers, totalCount } = await CustomerApi.getCustomers(
                paginationModel.page,
                paginationModel.pageSize
            );
            setCustomers(customers);
            setTotalRows(totalCount);
        };
        fetchCustomers();
    }, [paginationModel]);
    
    
    const columns = [
        { field: 'id', headerName: 'ID', width: 200 },
        { field: 'name', headerName: 'Name', width: 150 },
        { field: 'surname', headerName: 'Surname', width: 150 },
        { field: 'phone', headerName: 'Phone', width: 150 },
    ];
    
    const handlePaginationModelChange = (newPaginationModel: GridPaginationModel) => {
        setPaginationModel(newPaginationModel);
    };
    
    const handleOpenModal = () => {
        setNewCustomer({ name: '', surname: '', phone: '' });
        setOpenModal(true);
    };
    
    const handleCloseModal = () => {
        setOpenModal(false);
    };
    
    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setNewCustomer({ ...newCustomer, [name]: value });
    };
    
    const handleSubmit = async () => {
        try {
            const customer = await CustomerApi.createCustomer(newCustomer);
            setCustomers((prevCustomers) => [...prevCustomers, customer.customerDto]);
            setTotalRows(totalRows + 1);
            setOpenModal(false);
        } catch (error) {
            console.error('Error creating customer:', error);
        }
    };
    
    return (
        <div>
            <Button variant="contained" color="primary" onClick={handleOpenModal}
                    style={{ marginBottom: '16px' }}>
                Add New Customer
            </Button>
            <div style={{ height: 500, width: '100%' }}>
                <DataGrid
                    rows={customers}
                    columns={columns}
                    pageSizeOptions={[5, 10, 20]}
                    getRowId={(row) => row.id}
                    disableRowSelectionOnClick
                    rowCount={totalRows}
                    pagination
                    paginationMode="server"
                    paginationModel={paginationModel}
                    onPaginationModelChange={handlePaginationModelChange}
                />
            </div>
            
            <Dialog open={openModal} onClose={handleCloseModal}>
                <DialogTitle>Create New Customer</DialogTitle>
                <DialogContent>
                    <TextField
                        label="Name"
                        name="name"
                        value={newCustomer.name}
                        onChange={handleInputChange}
                        fullWidth
                        margin="normal"
                    />
                    <TextField
                        label="Surname"
                        name="surname"
                        value={newCustomer.surname}
                        onChange={handleInputChange}
                        fullWidth
                        margin="normal"
                    />
                    <TextField
                        label="Phone"
                        name="phone"
                        value={newCustomer.phone}
                        onChange={handleInputChange}
                        fullWidth
                        margin="normal"
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseModal} color="secondary">
                        Cancel
                    </Button>
                    <Button onClick={handleSubmit} color="primary">
                        Create
                    </Button>
                </DialogActions>
            </Dialog>
        </div>
    );
}