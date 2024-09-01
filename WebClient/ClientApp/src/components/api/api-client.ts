import axios, { AxiosRequestConfig, AxiosResponse } from "axios";
import { jwtDecode } from "jwt-decode";

import { Url } from "../constants/url";
import { LocalStorageKey } from "../constants/local-storage-key";

export class ApiClient {
    static baseUrl = process.env.REACT_APP_SERVER_URL;
    static authorizationHeader = "Authorization";
    private static refreshTokenPromise: Promise<any> | null = null;

    static async initialize() {
        axios.interceptors.response.use(
          (response) => response,
          ApiClient.handleRejectedRequest.bind(ApiClient)
        );
        
        const accessToken = localStorage.getItem(LocalStorageKey.AccessToken);
        if (accessToken) {
            axios.defaults.headers.common[this.authorizationHeader] = `Bearer ${accessToken}`;
            return true;
        } else {
            axios.defaults.headers.common[this.authorizationHeader] = null;
            return false;
        }
    }
    
    static async handleRejectedRequest(error: any) {
        if (error.response && error.response.status === 401) {
            await ApiClient.handleUnauthorizedRequest(error.config);
            return;
        } else if (error.response && error.response.status === 403) {
            window.location.href = "/";
        } else {
            return Promise.reject(error);
        }
    }
    
    private static async handleUnauthorizedRequest(config: AxiosRequestConfig) {
        if (!ApiClient.refreshTokenPromise) {
            ApiClient.refreshTokenPromise = ApiClient.refreshAccessToken()
              .finally(() => {
                  ApiClient.refreshTokenPromise = null;
              });
        }
        
        try {
            const resp = await ApiClient.refreshTokenPromise;
            config.headers![this.authorizationHeader] = `Bearer ${resp.data.accessToken}`;
            return axios(config);
        } catch (resp) {
            ApiClient.handleRefreshTokenFailure(resp);
        }
    }
    
    public static async refreshAccessToken() {
        try {
            const resp = await axios.post(`${this.baseUrl}/api/authentication/Refresh`, {}, { withCredentials: true });
            ApiClient.configureAuthorization(resp.data.accessToken);
            return resp;
        } catch (resp) {
            throw resp;
        }
    }
    
    private static handleRefreshTokenFailure(resp: any) {
        ApiClient.removeAuthorization();
        window.location.href = Url.Authentication.Logout;
        return Promise.reject(resp);
    }
    
    static async post(url: string, formData?: any, config?: AxiosRequestConfig) {
        return ApiClient.handleRequest(() => axios.post(this.baseUrl + url, formData, config));
    }
    
    static async put(url: string, formData?: any, config?: AxiosRequestConfig) {
        return ApiClient.handleRequest(() => axios.put(this.baseUrl + url, formData, config));
    }
    
    static async remove(url: string, content?: any) {
        return ApiClient.handleRequest(() => axios.delete(this.baseUrl + url, { data: content }));
    }
    
    static async get(url: string, config?: AxiosRequestConfig) {
        return await axios.get(this.baseUrl + url, config)
            .then((resp) => {
                return resp.data;
            })
            .catch((resp) => {
                if (resp.response !== undefined && resp.response.status === 401) {
                    ApiClient.removeAuthorization();
                    window.location.href = Url.Authentication.Logout;
                }
                return Promise.reject(resp);
            });
    }
    
    static async all(values: (AxiosResponse<any> | Promise<AxiosResponse<any>> | Promise<any>)[]): Promise<any[]> {
        return ApiClient.handleRequest(() => axios.all(values));
    }
    
    private static async handleRequest(requestFn: () => Promise<any>) {
        try {
            const resp = await requestFn();
            if (!resp || !resp.data) {
                return Promise.reject(resp);
            }
            return resp.data;
        } catch (resp) {
            if (resp && resp.response !== undefined && resp.response.status === 401) {
                await ApiClient.handleRefreshTokenFailure(resp);
            }
            return Promise.reject(resp);
        }
    }
    
    static configureAuthorization(accessToken: string) {
        axios.defaults.headers.common[this.authorizationHeader] = `Bearer ${accessToken}`;
        localStorage.setItem(LocalStorageKey.AccessToken, accessToken);
    }
    
    static removeAuthorization() {
        axios.defaults.headers.common[this.authorizationHeader] = null;
        localStorage.removeItem(LocalStorageKey.AccessToken);
    }
}