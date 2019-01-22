from setuptools import setup

setup(
    name='blackhole_data_gathering',
    version='0.1.0',
    description='A module for pulling data from IEX API using iexfinance module and pushing it to Azure',
    url='https://github.com/bodyionita/Project-BlackHole/tree/master/IEX%20StockMarket%20Data%20Gathering',
    license='GNU General Public License v3.0',
    author='Bogdan Ionita',
    author_email='bogdan.ionita.15@ucl.ac.uk',
    packages=['blackhole_data_gathering'],
    install_requires=['iexfinance'],
)
