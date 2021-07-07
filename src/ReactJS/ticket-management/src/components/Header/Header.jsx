import React from "react";
import s from './Header.module.css';
import {NavLink} from "react-router-dom";
import i18n from "i18next";
import {useTranslation} from "react-i18next";

const Header = (props) => {
    const {t} = useTranslation();
    const onChange = (props) => {
        i18n.changeLanguage(props.target.value);
    }
    return (
        <header className={s.header}>
            <div className={s.homeBlock}>
                <NavLink to={'/home'}>Home</NavLink>
            </div>
            <div>
                <select name={'language'} onChange={onChange}>
                    <option value='en'>English</option>
                    <option value='ru'>Русский</option>
                    <option value='be'>Белорусский</option>
                </select>
            </div>
            <div className={s.loginBlock}>
                {
                    props.isAuth ?
                        <div>
                            <span className={s.span}>{props.email}</span>
                            <span className={s.span}>
                                <button>
                                    {t('Login')}
                            </button>
                        </span>
                        </div>
                        : <span className={s.span}>
                    <NavLink to={'/login'}>
                    {t('Logout')}
                    </NavLink>
                    </span>
                }
            </div>
        </header>
    );
}

export default Header;