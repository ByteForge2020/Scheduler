import { createSlice } from '@reduxjs/toolkit'

interface AuthenticationState {
    value: boolean;
}

const initialState = { value: false } satisfies AuthenticationState as AuthenticationState


const authenticationSlice = createSlice({
    name: "authenticated",
    initialState,
    reducers: {
        authenticate: (state) => {
            state.value = true;
        },
        deauthenticate: (state) => {
            state.value = false;
        },
    },
});


export const { authenticate, deauthenticate } = authenticationSlice.actions;

export default authenticationSlice.reducer;