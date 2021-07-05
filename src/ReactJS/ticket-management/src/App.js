import './App.css';
import {Route} from "react-router-dom";
import EventsContainer from "./components/Events/EventsContainer";
import EventAreasContainer from "./components/EventAreas/EventAreasContainer";
import HeaderContainer from "./components/Header/HeaderContainer";

const App = () => {
    return (
            <div className='app-wrapper'>
                    <HeaderContainer />
                <div className='app-wrapper-content'>
                    <Route path='/home' render={() => <EventsContainer/>}/>
                    <Route path='/eventArea/:eventId' render={() => <EventAreasContainer/>}/>
                </div>
            </div>
    );
}


export default App;
