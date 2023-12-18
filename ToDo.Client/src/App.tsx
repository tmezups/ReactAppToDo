import './App.css';
import {Register, ToDo} from "./pages";
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import {Login} from "./pages/Login";

function App() {
   return (
       <BrowserRouter>
           <Routes>
               <Route path="/" element={ <ToDo />} />
               <Route path="/Login" element={ <Login />} />
               <Route path="/Register" element={ <Register />} />
           </Routes>
       </BrowserRouter>
   )
}

export default App;