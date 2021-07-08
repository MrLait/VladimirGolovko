import './App.css';
import {Route} from "react-router-dom";
import EventsContainer from "./components/Events/EventsContainer";
import EventAreasContainer from "./components/EventAreas/EventAreasContainer";
import HeaderContainer from "./components/Header/HeaderContainer";
import Login from "./components/Login/Login";
import Register from "./components/Register/Register";

const App = () => {
    return (
        <div className='app-wrapper'>
            <header className= 'header'><HeaderContainer/></header>
            <div className='content'>
                <Route path='/register' render={() => <Register/>}/>
                <Route path='/login' render={() => <Login/>}/>
                <Route path='/home' render={() => <EventsContainer/>}/>
                <Route path='/eventArea/:eventId' render={() => <EventAreasContainer/>}/>
            </div>
        </div>
    );
}


export default App;
