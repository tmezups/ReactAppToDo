import './App.css';
import {Register, ToDo} from "./pages";
import {BrowserRouter, Route, Routes} from 'react-router-dom';
import {Login} from "./pages";
import {UserContextProvider} from './Components/UserContext';
import {NavBar} from "./Components/Navbar.tsx";


function App() {
    return (
        <UserContextProvider>
            <BrowserRouter>
                <NavBar />
                <Routes>
                    <Route path="/" element={ <ToDo/> }/>
                    <Route path="/Login" element={<Login/>}/>
                    <Route path="/Register" element={<Register/>}/>
                </Routes>
            </BrowserRouter>
        </UserContextProvider>
    )
}

export default App;