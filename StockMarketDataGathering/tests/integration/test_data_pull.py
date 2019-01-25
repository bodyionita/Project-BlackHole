from blackhole_data_gathering.data_pull import DataPuller
from blackhole_data_gathering.util import read_from_json_file

import unittest
import shutil


class TestDataPull(unittest.TestCase):

    @classmethod
    def setUpClass(self):
        self.data_puller = DataPuller()
        self.data_dir = 'data/'
        self.subdir = 'test/'
        self.dir = self.data_dir + self.subdir

    @classmethod
    def tearDownClass(self):
        shutil.rmtree(self.dir)

    def test_pull_symbols(self):
        filename = 'test_symbols'
        self.data_puller.pull_symbols(filename=filename, subdir=self.subdir)
        symbols = read_from_json_file(filename=filename, subdir=self.subdir)

        self.assertEqual(symbols[0]['symbol'], 'A')

    def test_pull_symbols_extended(self):
        filename = 'test_symbols_extended'
        symbols = [{
                    "symbol": "A",
                    "name": "Agilent Technologies Inc.",
                    "date": "2019-01-14",
                    "isEnabled": True,
                    "type": "cs",
                    "iexId": "2"
                  }]
        data_verify = [{
                        "symbol": "A",
                        "companyName": "Agilent Technologies Inc.",
                        "exchange": "New York Stock Exchange",
                        "industry": "Medical Diagnostics & Research",
                        "website": "http://www.agilent.com",
                        "description": "Agilent Technologies Inc is engaged in life sciences, diagnostics and" +
                                       " applied chemical markets. The company provides application focused " +
                                       "solutions that include instruments, software, services and consumables" +
                                       " for the entire laboratory workflow.",
                        "CEO": "Michael R. McMullen",
                        "issueType": "cs",
                        "sector": "Healthcare",
                        "tags": [
                          "Healthcare",
                          "Diagnostics & Research",
                          "Medical Diagnostics & Research"
                        ]
                      }]

        self.data_puller.pull_symbols_extended(symbols=symbols, filename=filename, subdir=self.subdir)
        data = read_from_json_file(filename=filename, subdir=self.subdir)

        self.assertEqual(data_verify, data)


if __name__ == '__main__':
    unittest.main()
