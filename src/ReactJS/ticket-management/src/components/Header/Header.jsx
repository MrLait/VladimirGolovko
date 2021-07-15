import React from "react";
import s from './Header.module.css';
import {NavLink} from "react-router-dom";
import i18n from "i18next";
import {useTranslation} from "react-i18next";
import LanguageSelector from "../../common/Selectors/LanguageSelector";
import {roles} from "../Constants/userConst";
import {Redirect} from "react-router";

const Header = (props) => {
    const {t} = useTranslation();
    const onChange = (props) => {
        i18n.changeLanguage(props.target.value);
    }
    return (
        <header className={s.header}>
            <span className={s.homeBlock}>
                <NavLink to={'/home'}>{t('Home')}</NavLink>
            </span>
            <span className={s.authBlock}>
                {
                    props.isAuth ?
                        <span>
                            <span className={s.span}>
                                {
                                    props.role === roles.eventManager ?
                                        <NavLink to={'/eventManagerArea'}> {t('Event manager area')}</NavLink>
                                        : ""
                                }
                            </span>
                            <span className={s.span}>
                                <NavLink to={'/profile'}>
                                    {t('Profile')}
                                </NavLink>
                            </span>
                            <span className={s.span}>
                                <NavLink to={'/purchaseHistory'}>
                                    {t('Purchase history')}
                                </NavLink>
                            </span>
                            <span className={s.span}>
                                <NavLink to={'/basket'}>
                                    {t('Basket')}
                                </NavLink>
                            </span>
                            <span className={s.span}>{props.email}</span>
                            <span className={s.span}>
                                <button onClick={props.logout}>
                                    {t('Logout')}
                                </button>
                            </span>

                            <span className={s.select}>
                                <LanguageSelector {...props}/>
                            </span>
                        </span>
                        :
                        <span>
                        <span className={s.span}>
                            <NavLink to={'/register'}>
                                {t('Register')}
                            </NavLink>
                        </span>
                        <span className={s.span}>
                            <NavLink to={'/login'}>
                                {t('Login')}
                            </NavLink>
                        </span>
                        <select id={props.language} name={'language'} onChange={onChange} value={props.language}>
                            <option value='en'>English</option>
                            <option value='ru'>Русский</option>
                            <option value='be'>Белорусский</option>
                        </select>
                        </span>
                }
            </span>
        </header>
    );
}

export default Header;