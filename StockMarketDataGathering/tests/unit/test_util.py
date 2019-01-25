from blackhole_data_gathering.util import read_from_json_file, write_to_json_file

import unittest
import json
import shutil


class TestUtil(unittest.TestCase):

    @classmethod
    def setUpClass(self):
        self.data_dir = 'data/'
        self.subdir = 'test/'
        self.dir = self.data_dir + self.subdir
        self.filename = 'test_file'
        self.data = {'test': 1}

    @classmethod
    def tearDownClass(self):
        shutil.rmtree(self.dir)

    def test_1_read_inexistent_file(self):
        self.assertRaises(Exception, read_from_json_file, self.filename, self.subdir)

    def test_2_write_file(self):
        write_to_json_file(data=self.data, filename=self.filename, subdir=self.subdir)

        with open(self.dir + self.filename + '.json', 'r') as infile:
            loaded = json.load(infile)
            self.assertEqual(self.data, loaded)

    def test_3_read_file(self):
        loaded = read_from_json_file(filename=self.filename, subdir=self.subdir)
        self.assertEqual(self.data, loaded)


if __name__ == '__main__':
    unittest.main()
