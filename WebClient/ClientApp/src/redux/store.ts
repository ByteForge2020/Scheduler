import { configureStore } from "@reduxjs/toolkit";
import authenticationReducer from "./authentication-slice";

// const store = configureStore({
//   reducer: {
//     authentication: authenticationReducer,
//   },
//   middleware: (getDefaultMiddleware) =>
//     getDefaultMiddleware({
//       serializableCheck: false,
//     }),
//
// });

const store = configureStore({
  reducer: {
    authentication: authenticationReducer,
  }
});

export default store;

export const selectAuthenticationState = (state: RootState) => state.authentication.value;

// @ts-ignore
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
