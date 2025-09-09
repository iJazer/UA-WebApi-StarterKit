import { createTheme, ThemeOptions } from '@mui/material/styles';
import * as Color from '@mui/material/colors';

export enum ThemeModes {
   Light = 'light',
   Dark = 'dark'
}

declare module '@mui/material/styles' {
   interface TypographyVariants {
      bold: React.CSSProperties;
      error: React.CSSProperties;
      italic: React.CSSProperties;
      superscript: React.CSSProperties;
      subscript: React.CSSProperties;
      list: React.CSSProperties;
      table: React.CSSProperties;
   }

   // allow configuration using `createTheme`
   interface TypographyVariantsOptions {
      bold?: React.CSSProperties;
      error?: React.CSSProperties;
      italic?: React.CSSProperties;
      superscript?: React.CSSProperties;
      subscript?: React.CSSProperties;
      list?: React.CSSProperties;
      table?: React.CSSProperties;
   }
}

// Update the Typography's variant prop options
declare module '@mui/material/Typography' {
   interface TypographyPropsVariantOverrides {
      bold: true;
      error: true;
      italic: true;
      superscript: true;
      subscript: true;
      list: true;
      table: true;
   }
}

const lightText = '#FFFFFF'
const darkText = '#448FD1'
const mediumBlue = '#196096';
const lightBlue = '#E8EFF4';
const darkBlue = '#084A79';
const lightGrey = '#D7D9DA'

const mediumOrange = '#B45F06'

const darkThemeOverrides: ThemeOptions = {
   palette: {
      primary: Color.grey,
      divider: Color.grey[500],
      background: {
         default: Color.grey[900],
         paper: Color.grey[800],
      },
      text: {
         primary: Color.grey[50],
         secondary: Color.grey[500],
      },
   },
   components: {
      MuiContainer: {
         styleOverrides: {
            root: {
               maxWidth: 'none', // Override maxWidth here
            },
         },
      },
      MuiCssBaseline: {
         styleOverrides: `
               a { 
                  text-decoration: none;
               }
               a:link, a:visited, a:hover, a:active  { 
                  color: ${Color.grey[50]};
               }
            `
      },
      //MuiButton: {
      //   styleOverrides: {
      //      root: {
      //         backgroundColor: Color.grey[500],
      //         borderRadius: '0px',
      //         margin: '0px',
      //         minHeight: '0px',
      //         textTransform: 'unset',
      //         '&:hover': {
      //            backgroundColor: Color.grey[50],
      //         },
      //         '& a': {
      //            color: Color.grey[900]
      //         }
      //      }
      //   }
      //},
   }
};

const lightThemeOverrides: ThemeOptions = {
   palette: {
      primary: {
         main: mediumOrange,
         dark: darkBlue,
         light: lightBlue,
         contrastText: lightText
      },
      divider: darkBlue,
      text: {
         primary: '#000000',
         secondary: Color.grey[800]
      }
   },
   typography: {
      body1: {
         fontSize: '1.125rem',
         lineHeight: '1.55556',
         letterSpacing: '0.0125rem',
         textRendering: 'optimizeLegibility'
      },
      list: {
         marginBottom: "0"
      },
      table: {
         marginBottom: "0",
         paddingLeft: "0.5em",
         paddingRight: "0.5em"
      }
   },
   components: {
      MuiContainer: {
         styleOverrides: {
            root: {
               '@media (min-width:1280px)': {
                  maxWidth: 'none'
               },
               '@media (min-width:600px)': {
                  paddingLeft: '0px',
                  paddingRight: '0px'
               }
            },
         },
      },
      MuiCssBaseline: {
         styleOverrides: `
               a { 
                  text-decoration: none;
               }
               a:link, a:visited, a:hover, a:active  { 
                  color: ${darkText};
               }
               html {
                  scroll-behavior: smooth;
               }
               span {
                  white-space: pre-wrap;
               }
               div .scrollable {
                 overflow: auto;
                 scrollbar-width: thin; 
                 scrollbar-color: #888 #ddd;
               }
               div .scrollable::-webkit-scrollbar {
                 width: 6px;
               }
               div .scrollable::-webkit-scrollbar-track {
                 background: #ddd;
               }
               div .scrollable::-webkit-scrollbar-thumb {
                 background: #888;
               }
               tr:nth-of-type(even) {
                   background: #F2F2F2;
               }
               tr:nth-of-type(odd) {
                   background: #FFF;
               }
               th, td {
                   padding-left: 1rem;
                   padding-right: 1rem;
               }
            `
      },
      MuiButton: {
         styleOverrides: {
            root: {
               borderRadius: '0px',
               margin: '0px',
               minHeight: '0px',
               textTransform: 'unset',
               '&:hover': {
                  backgroundColor: Color.lightBlue[100],
                  color: Color.lightBlue[900]
               },
               '& a:hover': {
                  color: Color.lightBlue[900]
               },
               '& a': {
                  color: lightText
               },
               '& .MuiSvgIcon-root': {
                  color: lightText
               }
            }
         }
      },
      MuiTableCell: {
         styleOverrides: {
            head: {
               backgroundColor: lightGrey
            }
         }
      }
   }
};

const themeOverides = (mode: string): ThemeOptions => ({
   spacing: 1,
   ...((mode === ThemeModes.Light) ? lightThemeOverrides : darkThemeOverrides)
});

export const LightTheme = createTheme(themeOverides('light'));
export const DarkTheme = createTheme(themeOverides('dark'));

export default LightTheme;
