import './App.css';
import {Register, ToDo} from "./pages";
import {BrowserRouter, Route, Routes} from 'react-router-dom';
import {Login} from "./pages";
import {UserContext} from './UserContext';
import {useAuthenticatedApi} from "./useAuthenticatedApi.ts";

function App() {
    const userContext =  useAuthenticatedApi();
    return (
         <UserContext.Provider value={ userContext }> 
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={ userContext.user && <ToDo/>}/>
                    <Route path="/Login" element={<Login/>}/>
                    <Route path="/Register" element={<Register/>}/>
                </Routes>
            </BrowserRouter>
        </UserContext.Provider>
    )
}

export default App;