import { ApiClient } from './api-client';

export interface CustomerDto {
    id: string;
    name: string;
    surname: string;
    phone: string;
}
export interface CreateCustomerResponseDto {
    customerDto: CustomerDto;
}
export interface CreateCustomerDto {
    name: string;
    surname: string;
    phone: string;
}

export class CustomerApi {
    static async getCustomers(page: number, pageSize: number): Promise<{ customers: CustomerDto[]; totalCount: number }> {
        try {
            return await ApiClient.get(`/api/Customer/GetCustomers?page=${page}&pageSize=${pageSize}`);
        } catch (error) {
            console.error('Error fetching customers:', error);
            throw error;
        }
    }
    static async createCustomer(customer: CreateCustomerDto): Promise<CreateCustomerResponseDto> {
        try {
            const response = await ApiClient.post('/api/Customer/CreateCustomer', customer);
            return response;
        } catch (error) {
            console.error('Error creating customer:', error);
            throw error;
        }
    }
}